namespace DEPI.Core.Entities
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Location { get; set; }

        // --- الربط ---
        // بنقول للمستشفى: إنتي عندك لستة أدوية
        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}