public class UpdatePatientProfileDto
{
    // تغيير النوع من int لـ string ليتوافق مع المعرف الجديد (GUID)
    public string PatientId { get; set; } = string.Empty;

    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public string? ChronicDisease { get; set; }
    public string? NearestHospital { get; set; }
}