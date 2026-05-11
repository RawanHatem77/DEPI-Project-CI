using DEPI.Core.Interfaces;
using DEPI.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace DEPI.Infrastructure.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _cache;

        public AuthService(UserManager<IdentityUser> userManager, IMemoryCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<string> GenerateOtpAndSaveTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var otp = new Random().Next(100000, 999999).ToString();

            _cache.Set(otp, token, TimeSpan.FromMinutes(10));
            _cache.Set($"{otp}_email", email, TimeSpan.FromMinutes(10));

            return otp; 
        }

        public async Task<bool> ResetPasswordWithOtpAsync(string email, string otp, string newPassword)
        {
            if (_cache.TryGetValue(otp, out string? token) &&
                _cache.TryGetValue($"{otp}_email", out string? cachedEmail))
            {
                if (cachedEmail != email) return false;

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return false;

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                {
                    _cache.Remove(otp); 
                    return true;
                }
            }
            return false;
        }
    }
}