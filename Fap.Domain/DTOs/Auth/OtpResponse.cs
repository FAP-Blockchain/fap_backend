namespace Fap.Domain.DTOs.Auth
{
    public class OtpResponse
    {
        public string Email { get; set; }
        public string Purpose { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}