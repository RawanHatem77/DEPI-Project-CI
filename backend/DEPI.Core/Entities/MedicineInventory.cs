namespace DEPI.Core.Entities
{
    public class MedicineInventory
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = null!;

        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; } = null!;

        public int Quantity { get; set; }
        public DateTime? ExpectedArrival { get; set; } 
    }
}