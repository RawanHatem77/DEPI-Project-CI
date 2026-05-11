using DEPI.Core.DTOs;
using DEPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DEPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicinesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicines(string? name)
        {
            var query = _context.Medicines
                .Include(m => m.Hospital)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            var medicines = await query
                .Select(m => new {
                    m.Id,
                    MedicineName = m.Name,

                    HospitalName = m.Hospital != null ? m.Hospital.Name : "غير محدد",

                    m.Price,
                    m.Description,

                    Status = m.Quantity > 0 ? "متوفر" : "غير متوفر",

                    // التعديل هنا: بنقرأ التاريخ من الداتابيز لو موجود، لو مش موجود بنحط شرطة
                    Date = m.ExpectedDate.HasValue
                           ? m.ExpectedDate.Value.ToString("dd MMMM", new CultureInfo("ar-EG"))
                           : "-"
                })
                .ToListAsync();

            return Ok(medicines);
        }
        [HttpGet("by-hospital/{hospitalId}")]
        public async Task<IActionResult> GetMedicinesByHospital(int hospitalId)
        {
            // مثال داخل الـ Controller
            // في ميثود الـ GetMedicines أو ما يشابهها
            var medicines = await _context.Medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                Description = m.Description,
                Quantity = m.Quantity, // مهم جداً عشان الحالة تظهر "متوفر"
                ExpectedDate = m.ExpectedDate, // عشان التاريخ يظهر
                HospitalName = m.Hospital != null ? m.Hospital.Name : "غير محدد"
            }).ToListAsync();

            if (!medicines.Any())
            {
                return NotFound("عذراً، لا توجد أدوية متوفرة في هذه المستشفى حالياً.");
            }

            return Ok(medicines);
        }
    }
}