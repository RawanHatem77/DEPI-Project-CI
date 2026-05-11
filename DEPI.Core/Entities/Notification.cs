using System.ComponentModel.DataAnnotations.Schema;

namespace DEPI.Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public string PatientId { get; set; } = string.Empty;

        public string Title { get; set; } = "تنبيه جديد";

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public string? Type { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient? Patient { get; set; }
    }
}