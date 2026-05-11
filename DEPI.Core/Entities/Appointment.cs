using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEPI.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public string PatientId { get; set; } = null!;

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = null!;

        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; } = null!;

        public DateTime ReservationDate { get; set; } // تاريخ الحجز
        public TimeSpan ReservationTime { get; set; } // وقت الحجز

        public string Status { get; set; } = "Pending";

        // --- التعديل الجديد المطلوب ---
        // هذا الحقل ضروري جداً لعمل الإشعارات التلقائية
        // قيمته الافتراضية false، وعندما يرسل السيستم التنبيه تتحول لـ true
        public bool IsReminderSent { get; set; } = false;
    }
}