using DEPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HospitalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Hospitals
        [HttpGet]
        public async Task<IActionResult> GetHospitals()
        {
            // الأفضل نحدد الحقول اللي محتاجينها بس (Id, Name, Location) 
            // ده بيخلي الـ API أسرع ويمنع أي مشاكل في العلاقات الدائرية
            var hospitals = await _context.Hospitals
                .Select(h => new
                {
                    h.Id,
                    h.Name,
                    h.Location
                })
                .ToListAsync();

            if (hospitals == null || !hospitals.Any())
            {
                return NotFound("لا توجد مستشفيات مسجلة حالياً.");
            }

            return Ok(hospitals);
        }
    }
}