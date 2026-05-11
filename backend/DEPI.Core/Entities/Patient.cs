using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.Core.Entities
{
    public class Patient
    {
        // تم التغيير لـ string ليطابق الـ GUID الناتج عن الـ Identity
        // وأيضاً ليطابق نوع الـ PatientId في كلاس الـ Appointment
        public string Id { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ChronicDisease { get; set; } = string.Empty;
        public string NearestHospital { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
    }
}