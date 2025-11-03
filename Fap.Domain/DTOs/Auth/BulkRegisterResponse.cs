namespace Fap.Domain.DTOs.Auth
{
    public class BulkRegisterResponse
    {
        public int TotalRequested { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<RegisterUserResponse> Results { get; set; } = new List<RegisterUserResponse>();
    }
}