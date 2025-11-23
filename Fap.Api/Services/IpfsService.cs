using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Ipfs;
using Fap.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Fap.Api.Services
{
    /// <summary>
    /// Service for interacting with IPFS (InterPlanetary File System) via Pinata
    /// </summary>
    public class IpfsService : IIpfsService
    {
        private readonly IpfsSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<IpfsService> _logger;

        public IpfsService(
            IOptions<IpfsSettings> settings,
            HttpClient httpClient,
            ILogger<IpfsService> logger)
        {
            _settings = settings.Value;
            _httpClient = httpClient;
            _logger = logger;

            // Configure HttpClient for Pinata API
            _httpClient.BaseAddress = new Uri(_settings.ApiBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("pinata_api_key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("pinata_secret_api_key", _settings.ApiSecret);
        }

        /// <summary>
        /// Upload a file to IPFS via Pinata
        /// </summary>
        public async Task<string> UploadFileAsync(IFormFile file, string? fileName = null)
        {
            try
            {
                // Validate file size
                var maxSizeBytes = _settings.MaxFileSizeMB * 1024 * 1024;
                if (file.Length > maxSizeBytes)
                {
                    throw new InvalidOperationException(
                        $"File size exceeds maximum allowed size of {_settings.MaxFileSizeMB}MB");
                }

                using var content = new MultipartFormDataContent();
                using var fileStream = file.OpenReadStream();
                
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                
                var uploadFileName = fileName ?? file.FileName;
                content.Add(fileContent, "file", uploadFileName);

                // Add metadata
                var metadata = new
                {
                    name = uploadFileName,
                    keyvalues = new
                    {
                        uploadedAt = DateTime.UtcNow.ToString("O"),
                        originalFileName = file.FileName,
                        contentType = file.ContentType
                    }
                };
                content.Add(new StringContent(JsonSerializer.Serialize(metadata), Encoding.UTF8, "application/json"), "pinataMetadata");

                // Upload to Pinata
                var response = await _httpClient.PostAsync("/pinning/pinFileToIPFS", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var uploadResponse = JsonSerializer.Deserialize<PinataUploadResponse>(responseContent);

                if (uploadResponse == null || string.IsNullOrEmpty(uploadResponse.IpfsHash))
                {
                    throw new InvalidOperationException("Failed to get IPFS hash from Pinata response");
                }

                _logger.LogInformation(
                    "File uploaded to IPFS successfully. CID: {Cid}, Size: {Size} bytes",
                    uploadResponse.IpfsHash,
                    uploadResponse.PinSize);

                return uploadResponse.IpfsHash;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to IPFS: {FileName}", fileName ?? file.FileName);
                throw new InvalidOperationException($"Failed to upload file to IPFS: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Upload JSON metadata to IPFS
        /// </summary>
        public async Task<string> UploadMetadataAsync(object metadata, string? fileName = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var bytes = Encoding.UTF8.GetBytes(json);
                var metadataFileName = fileName ?? $"metadata_{DateTime.UtcNow:yyyyMMddHHmmss}.json";

                return await UploadBytesAsync(bytes, metadataFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading metadata to IPFS");
                throw new InvalidOperationException($"Failed to upload metadata to IPFS: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Upload raw bytes to IPFS
        /// </summary>
        public async Task<string> UploadBytesAsync(byte[] data, string fileName)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
            }

            // Validate file size
            var maxSizeBytes = _settings.MaxFileSizeMB * 1024 * 1024;
            if (data.Length > maxSizeBytes)
            {
                throw new InvalidOperationException(
                    $"Data size exceeds maximum allowed size of {_settings.MaxFileSizeMB}MB");
            }

            try
            {
                using var content = new MultipartFormDataContent();
                using var byteContent = new ByteArrayContent(data);
                
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(byteContent, "file", fileName);

                // Add metadata
                var metadata = new
                {
                    name = fileName,
                    keyvalues = new
                    {
                        uploadedAt = DateTime.UtcNow.ToString("O"),
                        size = data.Length
                    }
                };
                content.Add(new StringContent(JsonSerializer.Serialize(metadata), Encoding.UTF8, "application/json"), "pinataMetadata");

                var response = await _httpClient.PostAsync("/pinning/pinFileToIPFS", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var uploadResponse = JsonSerializer.Deserialize<PinataUploadResponse>(responseContent);

                if (uploadResponse == null || string.IsNullOrEmpty(uploadResponse.IpfsHash))
                {
                    throw new InvalidOperationException("Failed to get IPFS hash from Pinata response");
                }

                _logger.LogInformation(
                    "Bytes uploaded to IPFS successfully. CID: {Cid}, Size: {Size} bytes",
                    uploadResponse.IpfsHash,
                    data.Length);

                return uploadResponse.IpfsHash;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading bytes to IPFS: {FileName}", fileName);
                throw new InvalidOperationException($"Failed to upload bytes to IPFS: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get full gateway URL for accessing IPFS content
        /// </summary>
        public string GetFileUrl(string cid)
        {
            if (string.IsNullOrWhiteSpace(cid))
            {
                throw new ArgumentException("CID cannot be null or empty", nameof(cid));
            }

            return $"{_settings.GatewayUrl}{cid}";
        }

        /// <summary>
        /// Download file from IPFS
        /// </summary>
        public async Task<byte[]> DownloadFileAsync(string cid)
        {
            if (string.IsNullOrWhiteSpace(cid))
            {
                throw new ArgumentException("CID cannot be null or empty", nameof(cid));
            }

            try
            {
                var url = GetFileUrl(cid);
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsByteArrayAsync();
                
                _logger.LogInformation("File downloaded from IPFS. CID: {Cid}, Size: {Size} bytes", cid, content.Length);
                
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file from IPFS: {Cid}", cid);
                throw new InvalidOperationException($"Failed to download file from IPFS: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Unpin file from IPFS (remove from permanent storage)
        /// </summary>
        public async Task UnpinFileAsync(string cid)
        {
            if (string.IsNullOrWhiteSpace(cid))
            {
                throw new ArgumentException("CID cannot be null or empty", nameof(cid));
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"/pinning/unpin/{cid}");
                response.EnsureSuccessStatusCode();

                _logger.LogInformation("File unpinned from IPFS. CID: {Cid}", cid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unpinning file from IPFS: {Cid}", cid);
                throw new InvalidOperationException($"Failed to unpin file from IPFS: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get pin status and metadata for a file
        /// </summary>
        public async Task<object?> GetPinStatusAsync(string cid)
        {
            if (string.IsNullOrWhiteSpace(cid))
            {
                throw new ArgumentException("CID cannot be null or empty", nameof(cid));
            }

            try
            {
                var response = await _httpClient.GetAsync($"/data/pinList?hashContains={cid}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var pinData = JsonSerializer.Deserialize<object>(responseContent);

                return pinData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pin status from IPFS: {Cid}", cid);
                throw new InvalidOperationException($"Failed to get pin status from IPFS: {ex.Message}", ex);
            }
        }
    }
}
