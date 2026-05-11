using DEPI.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DEPI.Infrastructure.Data
{
    // بنخليه يورث من IdentityDbContext عشان يضيف جداول المستخدمين والـ Login لوحده
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        //public DbSet<MedicineInventory> Inventories { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicineInventory> MedicineInventories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. حل مشكلة الـ PatientId1 (اللي كنتِ عاملاها)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .IsRequired();

            // 2. حل مشكلة الـ Cascade Delete بين الدواء والمستشفى
            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.Hospital)
                .WithMany(h => h.Medicines)
                .HasForeignKey(m => m.HospitalId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Medicine>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");
           
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Patient) 
                .WithMany()
                .HasForeignKey(n => n.PatientId)
                .IsRequired();
          
            modelBuilder.Entity<Hospital>().HasData(
                new Hospital { Id = 1, Name = "مستشفى قصر العيني", Location = "المنيل، القاهرة" },
                new Hospital { Id = 2, Name = "مستشفى السلام الدولي", Location = "المعادي، القاهرة" },
                new Hospital { Id = 3, Name = "مستشفى عين شمس التخصصي", Location = "العباسية، القاهرة" },
                new Hospital { Id = 4, Name = "مستشفى كليوباترا", Location = "مصر الجديدة، القاهرة" },
                new Hospital { Id = 5, Name = "مستشفى معهد ناصر", Location = "كورنيش النيل، القاهرة" },
                new Hospital { Id = 6, Name = "مستشفى دار الفؤاد", Location = "طريق النصر، مدينة نصر" },
                new Hospital { Id = 7, Name = "المستشفى الجوي التخصصي", Location = "التجمع الخامس، القاهرة" }
            );

            // --- Seed Data للأدوية (لازم نبعت HospitalId و Price) ---
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { Id = 1, Name = "الأنسولين (ميكسيتارد)", Description = "سكر نوع أول", HospitalId = 1, Price = 150.0m, Quantity = 100 },
                new Medicine { Id = 2, Name = "لانتوس سولستار", Description = "أنسولين طويل المفعول", HospitalId = 5, Price = 450.0m, Quantity = 50 },
                new Medicine { Id = 3, Name = "سيدوفاج (ميتفورمين)", Description = "سكر نوع ثاني", HospitalId = 1, Price = 60.0m, Quantity = 200 },
                new Medicine { Id = 4, Name = "جالفس مت", Description = "منظم سكر مركب", HospitalId = 2, Price = 180.0m, Quantity = 80 },
                new Medicine { Id = 5, Name = "بلافيكس", Description = "سيولة الدم - مرضى القلب", HospitalId = 1, Price = 210.0m, Quantity = 90 },
                new Medicine { Id = 6, Name = "كونترولوك", Description = "حماية المعدة", HospitalId = 7, Price = 90.0m, Quantity = 110 },
                new Medicine { Id = 7, Name = "كونكور", Description = "ضغط عالي وقلب", HospitalId = 1, Price = 45.0m, Quantity = 150 },
                new Medicine { Id = 8, Name = "إيراستابكس", Description = "ضغط دم مرتفع", HospitalId = 7, Price = 120.0m, Quantity = 70 },
                new Medicine { Id = 9, Name = "كوراسور", Description = "ضغط دم منخفض", HospitalId = 3, Price = 30.0m, Quantity = 130 },
                new Medicine { Id = 10, Name = "ميدودرين", Description = "ضغط دم منخفض", HospitalId = 3, Price = 40.0m, Quantity = 100 },
                new Medicine { Id = 11, Name = "ميثوتريكسيت حقن", Description = "روماتيزم ومناعة", HospitalId = 3, Price = 300.0m, Quantity = 40 }
            );

            modelBuilder.Entity<MedicineInventory>().HasData(
                new MedicineInventory { Id = 1, HospitalId = 1, MedicineId = 1, Quantity = 150 },
                new MedicineInventory { Id = 2, HospitalId = 1, MedicineId = 5, Quantity = 80 },
                new MedicineInventory { Id = 3, HospitalId = 1, MedicineId = 7, Quantity = 200 },

                new MedicineInventory { Id = 11, HospitalId = 2, MedicineId = 4, Quantity = 80 }, 
                new MedicineInventory { Id = 12, HospitalId = 2, MedicineId = 5, Quantity = 40 }, 

                new MedicineInventory { Id = 4, HospitalId = 3, MedicineId = 11, Quantity = 40 },
                new MedicineInventory { Id = 5, HospitalId = 3, MedicineId = 9, Quantity = 100 },
                new MedicineInventory { Id = 13, HospitalId = 3, MedicineId = 10, Quantity = 50 }, 

                // مستشفى 4: كليوباترا (إضافة جديدة)
                new MedicineInventory { Id = 14, HospitalId = 4, MedicineId = 6, Quantity = 90 }, // كونترولوك
                new MedicineInventory { Id = 15, HospitalId = 4, MedicineId = 7, Quantity = 120 }, // كونكور

                // مستشفى 5: معهد ناصر (موجود مسبقاً)
                new MedicineInventory { Id = 6, HospitalId = 5, MedicineId = 1, Quantity = 500 },
                new MedicineInventory { Id = 7, HospitalId = 5, MedicineId = 2, Quantity = 300 },
                new MedicineInventory { Id = 8, HospitalId = 5, MedicineId = 3, Quantity = 400 },

                // مستشفى 6: دار الفؤاد (إضافة جديدة)
                new MedicineInventory { Id = 16, HospitalId = 6, MedicineId = 3, Quantity = 150 }, // سيدوفاج
                new MedicineInventory { Id = 17, HospitalId = 6, MedicineId = 5, Quantity = 60 },  // بلافيكس

                // مستشفى 7: المستشفى الجوي التخصصي (موجود مسبقاً)
                new MedicineInventory { Id = 9, HospitalId = 7, MedicineId = 8, Quantity = 60 },
                new MedicineInventory { Id = 10, HospitalId = 7, MedicineId = 6, Quantity = 120 }
            );
        }

    }
}