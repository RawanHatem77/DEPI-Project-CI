using DEPI.Core.Entities;
using DEPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetNotifications(string patientId)
        {
            var notifications = await _context.Notifications
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
        [HttpGet("unread-count/{patientId}")]
        public async Task<IActionResult> GetUnreadCount(string patientId)
        {
            var count = await _context.Notifications
                .CountAsync(n => n.PatientId == patientId && !n.IsRead);

            return Ok(new { unreadCount = count });
        }

        [HttpPatch("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return NotFound();

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم تحديث حالة الإشعار" });
        }

        [HttpPatch("mark-all-read/{patientId}")]
        public async Task<IActionResult> MarkAllRead(string patientId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.PatientId == patientId && !n.IsRead)
                .ToListAsync();

            foreach (var n in unreadNotifications)
            {
                n.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم قراءة جميع الإشعارات" });
        }
    }
}