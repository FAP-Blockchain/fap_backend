using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Ipfs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fap.Api.Controllers
{
    /// <summary>
    /// Controller for IPFS file operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IpfsController : ControllerBase
    {
        private readonly IIpfsService _ipfsService;
        private readonly ILogger<IpfsController> _logger;

        public IpfsController(
            IIpfsService ipfsService,
            ILogger<IpfsController> logger)
        {
            _ipfsService = ipfsService;
            _logger = logger;
        }

        /// <summary>
        /// Upload a file to IPFS
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns>IPFS upload result with CID and URL</returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(IpfsUploadResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IpfsUploadResult>> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file provided" });
                }

                _logger.LogInformation("Uploading file to IPFS: {FileName}, Size: {Size} bytes", 
                    file.FileName, file.Length);

                var cid = await _ipfsService.UploadFileAsync(file);
                var url = _ipfsService.GetFileUrl(cid);

                var result = new IpfsUploadResult
                {
                    Cid = cid,
                    Url = url,
                    Size = file.Length,
                    UploadedAt = DateTime.UtcNow,
                    FileName = file.FileName
                };

                _logger.LogInformation("File uploaded successfully. CID: {Cid}", cid);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument for file upload");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to IPFS");
                return StatusCode(500, new { message = "Failed to upload file to IPFS", error = ex.Message });
            }
        }

        /// <summary>
        /// Upload JSON metadata to IPFS
        /// </summary>
        /// <param name="metadata">Metadata object to upload</param>
        /// <returns>IPFS upload result with CID and URL</returns>
        [HttpPost("upload-metadata")]
        [ProducesResponseType(typeof(IpfsUploadResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IpfsUploadResult>> UploadMetadata([FromBody] object metadata)
        {
            try
            {
                if (metadata == null)
                {
                    return BadRequest(new { message = "No metadata provided" });
                }

                _logger.LogInformation("Uploading metadata to IPFS");

                var cid = await _ipfsService.UploadMetadataAsync(metadata);
                var url = _ipfsService.GetFileUrl(cid);

                var result = new IpfsUploadResult
                {
                    Cid = cid,
                    Url = url,
                    UploadedAt = DateTime.UtcNow,
                    FileName = "metadata.json"
                };

                _logger.LogInformation("Metadata uploaded successfully. CID: {Cid}", cid);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument for metadata upload");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading metadata to IPFS");
                return StatusCode(500, new { message = "Failed to upload metadata to IPFS", error = ex.Message });
            }
        }

        /// <summary>
        /// Get IPFS gateway URL for a given CID
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        /// <returns>Full gateway URL</returns>
        [HttpGet("url/{cid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public ActionResult<string> GetFileUrl(string cid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return BadRequest(new { message = "CID is required" });
                }

                var url = _ipfsService.GetFileUrl(cid);
                return Ok(new { cid, url });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid CID: {Cid}", cid);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting IPFS URL for CID: {Cid}", cid);
                return BadRequest(new { message = "Invalid CID", error = ex.Message });
            }
        }

        /// <summary>
        /// Download file from IPFS by CID
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        /// <returns>File content</returns>
        [HttpGet("download/{cid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DownloadFile(string cid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return BadRequest(new { message = "CID is required" });
                }

                _logger.LogInformation("Downloading file from IPFS. CID: {Cid}", cid);

                var fileContent = await _ipfsService.DownloadFileAsync(cid);
                
                return File(fileContent, "application/octet-stream", $"{cid}");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid CID for download: {Cid}", cid);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file from IPFS: {Cid}", cid);
                return StatusCode(500, new { message = "Failed to download file from IPFS", error = ex.Message });
            }
        }

        /// <summary>
        /// Unpin a file from IPFS
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        [HttpDelete("unpin/{cid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UnpinFile(string cid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return BadRequest(new { message = "CID is required" });
                }

                _logger.LogInformation("Unpinning file from IPFS. CID: {Cid}", cid);

                await _ipfsService.UnpinFileAsync(cid);
                
                return Ok(new { message = "File unpinned successfully", cid });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid CID for unpin: {Cid}", cid);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unpinning file from IPFS: {Cid}", cid);
                return StatusCode(500, new { message = "Failed to unpin file from IPFS", error = ex.Message });
            }
        }

        /// <summary>
        /// Get pin status for a file
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        [HttpGet("pin-status/{cid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPinStatus(string cid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return BadRequest(new { message = "CID is required" });
                }

                _logger.LogInformation("Getting pin status for CID: {Cid}", cid);

                var status = await _ipfsService.GetPinStatusAsync(cid);
                
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid CID for pin status: {Cid}", cid);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pin status for CID: {Cid}", cid);
                return StatusCode(500, new { message = "Failed to get pin status", error = ex.Message });
            }
        }
    }
}
