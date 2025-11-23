using System.Text.Json.Serialization;

namespace Fap.Domain.DTOs.Ipfs
{
    /// <summary>
    /// Response from Pinata API when uploading a file
    /// </summary>
    public class PinataUploadResponse
    {
        [JsonPropertyName("IpfsHash")]
        public string IpfsHash { get; set; } = string.Empty;

        [JsonPropertyName("PinSize")]
        public long PinSize { get; set; }

        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("isDuplicate")]
        public bool IsDuplicate { get; set; }
    }

    /// <summary>
    /// Response containing IPFS upload details
    /// </summary>
    public class IpfsUploadResult
    {
        /// <summary>
        /// IPFS Content Identifier (Hash)
        /// </summary>
        public string Cid { get; set; } = string.Empty;

        /// <summary>
        /// Full URL to access the file via IPFS gateway
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Upload timestamp
        /// </summary>
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Original file name
        /// </summary>
        public string? FileName { get; set; }
    }

    /// <summary>
    /// Pinata pin list response item
    /// </summary>
    public class PinataPin
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ipfs_pin_hash")]
        public string IpfsPinHash { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("date_pinned")]
        public DateTime DatePinned { get; set; }

        [JsonPropertyName("date_unpinned")]
        public DateTime? DateUnpinned { get; set; }

        [JsonPropertyName("metadata")]
        public PinataMetadata? Metadata { get; set; }
    }

    public class PinataMetadata
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("keyvalues")]
        public Dictionary<string, string>? KeyValues { get; set; }
    }
}
