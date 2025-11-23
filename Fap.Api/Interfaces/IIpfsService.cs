using Microsoft.AspNetCore.Http;

namespace Fap.Api.Interfaces
{
    /// <summary>
    /// Interface for IPFS (InterPlanetary File System) operations
    /// Used for decentralized file storage
    /// </summary>
    public interface IIpfsService
    {
        /// <summary>
        /// Upload a file to IPFS and return the CID (Content Identifier Hash)
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="fileName">Optional custom file name</param>
        /// <returns>IPFS CID (Hash)</returns>
        Task<string> UploadFileAsync(IFormFile file, string? fileName = null);

        /// <summary>
        /// Upload JSON metadata to IPFS (useful for storing structured data like transcripts)
        /// </summary>
        /// <param name="metadata">Object to serialize to JSON and upload</param>
        /// <param name="fileName">Optional metadata file name</param>
        /// <returns>IPFS CID (Hash)</returns>
        Task<string> UploadMetadataAsync(object metadata, string? fileName = null);

        /// <summary>
        /// Upload raw bytes to IPFS
        /// </summary>
        /// <param name="data">Byte array to upload</param>
        /// <param name="fileName">File name</param>
        /// <returns>IPFS CID (Hash)</returns>
        Task<string> UploadBytesAsync(byte[] data, string fileName);

        /// <summary>
        /// Get the full IPFS gateway URL for a given CID
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        /// <returns>Full URL to access the file</returns>
        string GetFileUrl(string cid);

        /// <summary>
        /// Download file from IPFS by CID
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        /// <returns>File content as byte array</returns>
        Task<byte[]> DownloadFileAsync(string cid);

        /// <summary>
        /// Unpin a file from IPFS (remove from permanent storage)
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        Task UnpinFileAsync(string cid);

        /// <summary>
        /// Get metadata about a pinned file
        /// </summary>
        /// <param name="cid">IPFS Content Identifier</param>
        /// <returns>Pinned file metadata</returns>
        Task<object?> GetPinStatusAsync(string cid);
    }
}
