using System.ComponentModel.DataAnnotations;

namespace DEPI.Core.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "الإيميل مطلوب")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string? Password { get; set; }
    }
}