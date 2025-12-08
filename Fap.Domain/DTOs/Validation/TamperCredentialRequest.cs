namespace Fap.Domain.DTOs.Validation
{
    public class TamperCredentialRequest
    {
        public string FileUrl { get; set; } = string.Empty;
        public string? IPFSHash { get; set; }
    }
}
