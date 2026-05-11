using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.Core.DTOs
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public string? MedicineName { get; set; }
        public string? HospitalName { get; set; }
        public string? Status { get; set; }
        public string? FormattedDate { get; set; } // للتاريخ
        public string? FormattedTime { get; set; }
        public string? DayLabel { get; set; } 
    }
}
