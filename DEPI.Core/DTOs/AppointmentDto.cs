namespace DEPI.Core.DTOs
{
    public class AppointmentDto
    {
        public string? PatientId { get; set; }
        public int HospitalId { get; set; }
        public int MedicineId { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}