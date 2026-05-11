using DEPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly AppDbContext _context; // استبدليها باسم الـ DbContext عندك

    public PatientController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdatePatientProfileDto model)
    {
        var patient = await _context.Patients.FindAsync(model.PatientId);

        if (patient == null)
            return NotFound("المريض غير موجود");

        // تحديث البيانات مع التأكد إن القيم مش Null قبل التخزين
        if (!string.IsNullOrEmpty(model.Name)) patient.Name = model.Name;
        if (model.Age.HasValue) patient.Age = model.Age.Value;
        if (!string.IsNullOrEmpty(model.Gender)) patient.Gender = model.Gender;
        if (!string.IsNullOrEmpty(model.ChronicDisease)) patient.ChronicDisease = model.ChronicDisease;
        if (!string.IsNullOrEmpty(model.NearestHospital)) patient.NearestHospital = model.NearestHospital;

        await _context.SaveChangesAsync();

        return Ok(new { message = "تم تحديث البيانات بنجاح" });
    }
    [HttpGet("get-profile/{id}")]
    public async Task<IActionResult> GetProfile(string id) // تم التغيير من int لـ string ليتوافق مع نظام الـ Identity
    {
        // البحث عن المريض باستخدام الـ ID النصي
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null)
            return NotFound("المريض غير موجود");

        // إرجاع البيانات لعرضها في واجهة المستخدم (UI)
        return Ok(new
        {
            patient.Name,
            patient.Age,
            patient.Gender,
            patient.ChronicDisease,
            patient.NearestHospital
        });
    }
    [HttpGet("appointments-history/{id}")]
    public async Task<IActionResult> GetAppointmentsHistory(string id) // تغيير من int لـ string
    {
        var history = await _context.Appointments
            .Where(r => r.PatientId == id) // الآن المقارنة ستنجح لأن الطرفين string
            .Select(r => new {
                MedicineName = r.Medicine.Name,
                HospitalName = r.Hospital.Name,
                Date = r.ReservationDate.ToString("yyyy-MM-dd"),
                Time = r.ReservationTime
            })
            .ToListAsync();

        return Ok(history);
    }
}