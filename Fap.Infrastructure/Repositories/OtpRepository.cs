using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class OtpRepository : GenericRepository<Otp>, IOtpRepository
    {
        public OtpRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<Otp?> GetValidOtpAsync(string email, string code, string purpose)
        {
            return await _dbSet
                .Where(o => o.Email == email 
                         && o.Code == code 
                         && o.Purpose == purpose 
                         && !o.IsUsed
                         && o.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Otp>> GetActiveOtpsByEmailAsync(string email, string purpose)
        {
            return await _dbSet
                .Where(o => o.Email == email 
                         && o.Purpose == purpose 
                         && !o.IsUsed
                         && o.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Otp>> GetExpiredOtpsAsync(int daysOld)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            return await _dbSet
                .Where(o => (o.ExpiresAt < DateTime.UtcNow || o.IsUsed)
                         && o.CreatedAt < cutoffDate)
                .ToListAsync();
        }

        public async Task InvalidateOtpsAsync(string email, string purpose)
        {
            var otps = await _dbSet
                .Where(o => o.Email == email && o.Purpose == purpose && !o.IsUsed)
                .ToListAsync();

            foreach (var otp in otps)
            {
                otp.IsUsed = true;
                otp.UsedAt = DateTime.UtcNow;
            }
        }
    }
}