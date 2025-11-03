using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Fap.Api.Services
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string email, string purpose);
        Task<bool> ValidateOtpAsync(string email, string code, string purpose);
        Task CleanupExpiredOtpsAsync();
    }

    public class OtpService : IOtpService
    {
        private readonly IUnitOfWork _uow;
        private readonly OtpSettings _otpSettings;
        private readonly ILogger<OtpService> _logger;

        public OtpService(
            IUnitOfWork uow, 
            IOptions<OtpSettings> otpSettings, 
            ILogger<OtpService> logger)
        {
            _uow = uow;
            _otpSettings = otpSettings.Value;
            _logger = logger;
        }

        public async Task<string> GenerateOtpAsync(string email, string purpose)
        {
            // Invalidate old OTPs for same email and purpose
            await _uow.Otps.InvalidateOtpsAsync(email, purpose);
            await _uow.SaveChangesAsync();

            // Generate new OTP
            var code = GenerateRandomCode(_otpSettings.Length);
            var otp = new Otp
            {
                Id = Guid.NewGuid(),
                Email = email,
                Code = code,
                Purpose = purpose,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_otpSettings.ExpirationMinutes),
                IsUsed = false
            };

            await _uow.Otps.AddAsync(otp);
            await _uow.SaveChangesAsync();

            _logger.LogInformation($"✅ OTP generated for {email} - Purpose: {purpose}");
            return code;
        }

        public async Task<bool> ValidateOtpAsync(string email, string code, string purpose)
        {
            var otp = await _uow.Otps.GetValidOtpAsync(email, code, purpose);

            if (otp == null)
            {
                _logger.LogWarning($"❌ Invalid OTP for {email} - Purpose: {purpose}");
                return false;
            }

            // Mark as used
            otp.IsUsed = true;
            otp.UsedAt = DateTime.UtcNow;
            _uow.Otps.Update(otp);
            await _uow.SaveChangesAsync();

            _logger.LogInformation($"✅ OTP validated successfully for {email}");
            return true;
        }

        public async Task CleanupExpiredOtpsAsync()
        {
            var expiredOtps = await _uow.Otps.GetExpiredOtpsAsync(7); // Keep for 7 days for audit

            foreach (var otp in expiredOtps)
            {
                _uow.Otps.Remove(otp);
            }

            var deletedCount = await _uow.SaveChangesAsync();
            _logger.LogInformation($"🗑️ Cleaned up {deletedCount} expired OTPs");
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "0123456789";
            var result = new char[length];
            
            using (var rng = RandomNumberGenerator.Create())
            {
                var buffer = new byte[length];
                rng.GetBytes(buffer);
                
                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[buffer[i] % chars.Length];
                }
            }
            
            return new string(result);
        }
    }
}