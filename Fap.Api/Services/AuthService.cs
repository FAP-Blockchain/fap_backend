using AutoMapper;
using Fap.Domain.DTOs.Auth;
using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fap.Api.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthService(IUnitOfWork uow, IConfiguration config, IMapper mapper)
        {
            _uow = uow;
            _config = config;
            _mapper = mapper;
        }

        // 🔑 LOGIN
        public async Task<LoginResponse?> LoginAsync(LoginRequest req)
        {

            var user = await _uow.Users.GetByEmailAsync(req.Email);

            if (user == null || !user.IsActive) return null;

           
            if (user.Role == null)
            {
                user = await _uow.Users.GetByIdWithRoleAsync(user.Id);
                if (user?.Role == null)
                    throw new Exception($"User {user?.Email} không có Role hoặc RoleId trỏ sai.");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            // generate JWT + RefreshToken
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await CreateRefreshTokenAsync(user);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Role = user.Role.Name,
                FullName = user.FullName
            };
        }

        // 🔓 LOGOUT
        public async Task<bool> LogoutAsync(Guid userId)
        {
            var tokens = await _uow.RefreshTokens.FindAsync(r => r.UserId == userId);
            foreach (var token in tokens)
                _uow.RefreshTokens.Remove(token);

            await _uow.SaveChangesAsync();
            return true;
        }

        // 🔁 RESET PASSWORD
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest req)
        {
            if (req.NewPassword != req.ConfirmPassword)
                throw new Exception("Password confirmation does not match");

            var user = (await _uow.Users.FindAsync(u => u.Email == req.Email)).FirstOrDefault();
            if (user == null) return false;

            user.PasswordHash = _hasher.HashPassword(user, req.NewPassword);
            _uow.Users.Update(user);
            await _uow.SaveChangesAsync();
            return true;
        }

        // ========== Private Helpers ==========
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };


            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(User user)
        {
            var token = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid().ToString("N"),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _uow.RefreshTokens.AddAsync(token);
            await _uow.SaveChangesAsync();
            return token;
        }
    }
}
