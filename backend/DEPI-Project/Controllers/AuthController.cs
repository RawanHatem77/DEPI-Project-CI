using DEPI.Core.DTOs;
using DEPI.Core.Entities;
using DEPI.Core.Interfaces;
using DEPI.Infrastructure.Data; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthService _authService;
        private readonly AppDbContext _context; 
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthController(
      UserManager<IdentityUser> userManager,
      IAuthService authService,
      AppDbContext context,
      IWebHostEnvironment webHostEnvironment) 
        {
            _userManager = userManager;
            _authService = authService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                var patient = new Patient
                {
                    Id = user.Id,
                    Name = model.Email!, 
                    Age = 0,
                    Gender = "Not Specified",
                    ChronicDisease = "None",
                    NearestHospital = "None"
                };

                try
                {
                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "تم إنشاء الحساب وتفعيله بنجاح" });
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user);
                    return StatusCode(500, "حدث خطأ أثناء حفظ بيانات المريض.");
                }
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email!);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Id == user.Id);

                if (patient != null)
                {
                    return Ok(new
                    {
                        Message = "أهلاً بيك، تم تسجيل الدخول بنجاح!",
                        PatientId = patient.Id,
                        UserId = user.Id,
                        Email = user.Email,
                        Name = patient.Name,
                        ProfilePicture = patient.ProfilePicture
                    });
                }

                return NotFound("بيانات المريض غير مكتملة، يرجى مراجعة الإدارة.");
            }

            return Unauthorized("الإيميل أو كلمة المرور غير صحيحة");
        }

        [HttpGet("get-profile/{patientId}")]
        public async Task<IActionResult> GetProfile(string patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);

            if (patient == null)
                return NotFound("لم يتم العثور على بيانات المريض.");

            return Ok(new
            {
                patient.Id,
                patient.Name,
                patient.Age,
                patient.Gender,
                patient.ChronicDisease,
                patient.NearestHospital,
                patient.ProfilePicture // ده اللي هيخلي الصورة تظهر فوراً
            });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var otp = await _authService.GenerateOtpAndSaveTokenAsync(email);
            if (otp == null) return BadRequest("الإيميل غير موجود");
            return Ok(new { Message = "تم إرسال كود التحقق بنجاح", OTP = otp });
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithOtpDto model)
        {
            var result = await _authService.ResetPasswordWithOtpAsync(model.Email!, model.OTP!, model.NewPassword!);
            if (result) return Ok(new { Message = "تم تغيير كلمة المرور بنجاح!" });
            return BadRequest("كود التحقق خاطئ أو منتهي الصلاحية");
        }

        [HttpPost("upload-profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadImageDto model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("يرجى اختيار صورة صحيحة.");

            // تحديد مسار الـ wwwroot
            string wwwRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "profile_pictures");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // توليد اسم فريد للصورة
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }

            // البحث عن المريض باستخدام الـ Id (اللي هو الـ GUID)
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == model.PatientId);

            if (patient != null)
            {
                // حفظ المسار في الداتابيز
                patient.ProfilePicture = "/uploads/profile_pictures/" + fileName;
                await _context.SaveChangesAsync();

                return Ok(new { filePath = patient.ProfilePicture });
            }

            return NotFound("المريض غير موجود.");
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdatePatientProfileDto model)
        {
            // 1. البحث عن المريض بالـ ID (الـ GUID)
            var patient = await _context.Patients.FindAsync(model.PatientId);

            if (patient == null)
                return NotFound("عذراً، المريض غير موجود.");

            try
            {
                // 2. التحديث الذكي: لو الحقل جاي فيه قيمة، حدّثه.. لو null، سيب القديم زي ما هو
                patient.Name = model.Name ?? patient.Name;
                patient.Age = model.Age ?? patient.Age;
                patient.Gender = model.Gender ?? patient.Gender;
                patient.ChronicDisease = model.ChronicDisease ?? patient.ChronicDisease;
                patient.NearestHospital = model.NearestHospital ?? patient.NearestHospital;

                // 3. حفظ التغييرات
                await _context.SaveChangesAsync();

                return Ok(new { message = "تم تحديث بياناتك بنجاح يا بطلة!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"حصلت مشكلة وأنا بحفظ البيانات: {ex.Message}");
            }
        }
        // ضيفي الـ Class دي تحت الـ Controller أو في ملف الـ DTOs
        public class UploadImageDto
        {
            public IFormFile? File { get; set; }
            public string? PatientId { get; set; }
            public string? PatientEmail { get; set; }
        }
    }
}