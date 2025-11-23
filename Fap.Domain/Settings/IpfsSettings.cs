namespace Fap.Domain.Settings
{
    /// <summary>
    /// IPFS configuration settings for decentralized file storage
    /// </summary>
    public class IpfsSettings
    {
        /// <summary>
        /// IPFS provider (Pinata, Infura, or SelfHosted)
        /// </summary>
        public string Provider { get; set; } = "Pinata";

        /// <summary>
        /// Pinata API Key
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Pinata API Secret
        /// </summary>
        public string ApiSecret { get; set; } = string.Empty;

        /// <summary>
        /// IPFS Gateway URL for retrieving files
        /// </summary>
        public string GatewayUrl { get; set; } = "https://gateway.pinata.cloud/ipfs/";

        /// <summary>
        /// Pinata API Base URL
        /// </summary>
        public string ApiBaseUrl { get; set; } = "https://api.pinata.cloud";

        /// <summary>
        /// Maximum file size in MB (default: 100MB)
        /// </summary>
        public int MaxFileSizeMB { get; set; } = 100;

        /// <summary>
        /// Enable file pinning (keep files permanently on IPFS)
        /// </summary>
        public bool EnablePinning { get; set; } = true;
    }
}
