using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface IOtpRepository : IGenericRepository<Otp>
    {
        Task<Otp?> GetValidOtpAsync(string email, string code, string purpose);
        Task<List<Otp>> GetActiveOtpsByEmailAsync(string email, string purpose);
        Task<List<Otp>> GetExpiredOtpsAsync(int daysOld);
        Task InvalidateOtpsAsync(string email, string purpose);
    }
}