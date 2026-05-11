namespace DEPI.Core.Entities
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ActiveIngredient { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public decimal Price { get; set; } // ضيفيه لو مش موجود عشان الفرونت

        public int HospitalId { get; set; }

        public Hospital Hospital { get; set; } = null!;
    }
}