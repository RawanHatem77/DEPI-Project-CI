using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.Core.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Message { get; set; }

        // هنا ممكن نبعت التاريخ متنسق وجاهز للعرض
        public string CreatedAtString { get; set; } = string.Empty;

        public bool IsRead { get; set; }
    }
}