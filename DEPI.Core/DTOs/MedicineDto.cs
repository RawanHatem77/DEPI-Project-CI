using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.Core.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? HospitalName { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int Quantity { get; set; }
    }
}
