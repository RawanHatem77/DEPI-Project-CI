namespace DEPI.Core.DTOs
{
    public class ResetPasswordWithOtpDto
    {
        public string? Email { get; set; }
        public string? OTP { get; set; }
        public string? NewPassword { get; set; }
    }
}