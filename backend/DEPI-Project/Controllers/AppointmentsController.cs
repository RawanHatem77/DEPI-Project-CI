using DEPI.Core.DTOs;
using DEPI.Infrastructure.Data;
using DEPI.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. حجز موعد مع تحديث المخزن وإرسال إشعار فوري
        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] AppointmentDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.PatientId))
                return BadRequest("بيانات المريض أو الموعد غير مكتملة");

            var inventory = await _context.MedicineInventories
                .Include(mi => mi.Hospital) // لتضمين اسم المستشفى في الإشعار
                .FirstOrDefaultAsync(mi => mi.MedicineId == model.MedicineId && mi.HospitalId == model.HospitalId);

            if (inventory == null || inventory.Quantity <= 0)
                return BadRequest("عذراً، هذا الدواء غير متوفر حالياً في هذه المستشفى.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var appointment = new Appointment
                {
                    PatientId = model.PatientId,
                    HospitalId = model.HospitalId,
                    MedicineId = model.MedicineId,
                    ReservationDate = model.AppointmentDate.Date,
                    ReservationTime = model.AppointmentDate.TimeOfDay,
                    Status = "Pending"
                };

                _context.Appointments.Add(appointment);
                inventory.Quantity -= 1;

                await _context.SaveChangesAsync();

                // إضافة إشعار تأكيد الحجز فوراً
                var bookingNotification = new Notification
                {
                    PatientId = appointment.PatientId,
                    Title = "تأكيد حجز موعد",
                    Message = $"تم حجز موعدك بنجاح في {inventory.Hospital?.Name} ليوم {appointment.ReservationDate.ToString("dd/MM/yyyy")} الساعة {appointment.ReservationTime.ToString(@"hh\:mm")}.",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Type = "Success"
                };
                _context.Notifications.Add(bookingNotification);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Message = "تم حجز الموعد بنجاح وتلقيت إشعاراً جديداً!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"حدث خطأ أثناء الحجز: {ex.Message}");
            }
        }

        // 2. جلب الأدوية المتاحة لمستشفى معين
        [HttpGet("available-medicines/{hospitalId}")]
        public async Task<IActionResult> GetAvailableMedicines(int hospitalId)
        {
            var medicines = await _context.MedicineInventories
                .AsNoTracking()
                .Where(mi => mi.HospitalId == hospitalId && mi.Quantity > 0)
                .Include(mi => mi.Medicine)
                .Select(mi => new {
                    Id = mi.Medicine.Id,
                    Name = mi.Medicine.Name,
                    Description = mi.Medicine.Description,
                    Price = mi.Medicine.Price,
                    Status = "متوفر"
                })
                .ToListAsync();

            if (!medicines.Any())
                return NotFound("عذراً، لا توجد أدوية متوفرة في هذه المستشفى حالياً.");

            return Ok(medicines);
        }

        // 3. جلب المواعيد المتاحة (Slots)
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(int hospitalId, DateTime date)
        {
            var allSlots = new List<TimeSpan>
            {
                new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0), new TimeSpan(12, 0, 0),
                new TimeSpan(13, 0, 0), new TimeSpan(14, 0, 0)
            };

            var bookedSlots = await _context.Appointments
                .AsNoTracking()
                .Where(a => a.HospitalId == hospitalId && a.ReservationDate.Date == date.Date)
                .Select(a => a.ReservationTime)
                .ToListAsync();

            var availableSlots = allSlots.Where(slot => !bookedSlots.Contains(slot))
                .Select(slot => slot.ToString(@"hh\:mm"))
                .ToList();

            return Ok(availableSlots);
        }

        // 4. إلغاء الموعد وإعادة الكمية للمخزن وإرسال إشعار إلغاء
        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound("الموعد غير موجود");

            var inventory = await _context.MedicineInventories
                .FirstOrDefaultAsync(mi => mi.HospitalId == appointment.HospitalId && mi.MedicineId == appointment.MedicineId);

            if (inventory != null)
                inventory.Quantity += 1;

            // إضافة إشعار الإلغاء
            var cancelNotification = new Notification
            {
                PatientId = appointment.PatientId,
                Title = "إلغاء موعد",
                Message = $"تم إلغاء موعدك المقرر بتاريخ {appointment.ReservationDate.ToString("dd/MM/yyyy")} بنجاح.",
                CreatedAt = DateTime.Now,
                IsRead = false,
                Type = "Warning"
            };
            _context.Notifications.Add(cancelNotification);

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "تم إلغاء الموعد بنجاح وتحديث المخزن." });
        }

        // 5. عرض مواعيد المريض
        [HttpGet("my-appointments/{patientId}")]
        public async Task<IActionResult> GetPatientAppointments(string patientId)
        {
            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Hospital)
                .Include(a => a.Medicine)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ReservationDate)
                .Select(a => new AppointmentResponseDto
                {
                    Id = a.Id,
                    HospitalName = a.Hospital.Name,
                    MedicineName = a.Medicine.Name,
                    Status = a.Status,
                    FormattedDate = a.ReservationDate.ToString("dd/MM/yyyy"),
                    FormattedTime = a.ReservationTime.ToString(@"hh\:mm"),
                    DayLabel = a.ReservationDate.Date == DateTime.Now.Date.AddDays(1) ? "غداً" : ""
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // 6. إحصائيات لوحة التحكم
        [HttpGet("dashboard-stats/{patientId}")]
        public async Task<IActionResult> GetDashboardStats(string patientId)
        {
            var activeAppointmentsCount = await _context.Appointments
                .AsNoTracking()
                .CountAsync(a => a.PatientId == patientId && (a.ReservationDate >= DateTime.Now.Date));

            var nextAppointment = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Hospital)
                .Where(a => a.PatientId == patientId && (a.ReservationDate >= DateTime.Now.Date))
                .OrderBy(a => a.ReservationDate)
                .ThenBy(a => a.ReservationTime)
                .Select(a => new {
                    Date = a.ReservationDate.ToString("dd/MM/yyyy"),
                    Time = a.ReservationTime.ToString(@"hh\:mm"),
                    HospitalName = a.Hospital.Name
                })
                .FirstOrDefaultAsync();

            return Ok(new
            {
                ActiveCount = activeAppointmentsCount,
                NextAppointment = nextAppointment
            });
        }

        // 7. التنبيهات (يفضل استخدام NotificationsController مستقبلاً للتحكم الكامل)
        [HttpGet("notifications/{patientId}")]
        public async Task<IActionResult> GetNotifications(string patientId)
        {
            var notifications = await _context.Notifications
                .AsNoTracking()
                .Where(n => n.PatientId == patientId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new {
                    n.Id,
                    n.Title,
                    n.Message,
                    n.Type,
                    n.IsRead,
                    Date = n.CreatedAt.ToString("yyyy-MM-dd HH:mm")
                })
                .ToListAsync();

            return Ok(notifications);
        }
    }
}