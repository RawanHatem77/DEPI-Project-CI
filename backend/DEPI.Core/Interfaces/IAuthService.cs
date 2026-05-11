using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateOtpAndSaveTokenAsync(string email);
        Task<bool> ResetPasswordWithOtpAsync(string email, string otp, string newPassword);
    }
}
