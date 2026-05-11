using DEPI.Core.Interfaces;
using DEPI.Infrastructure.Data;
using DEPI.Infrastructure.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DEPI_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- التعديل الأخير: إعدادات Kestrel لزيادة المهلة وحجم البيانات ---
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 104857600; // 100MB
                serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
            });

            // 1. إعداد قاعدة البيانات
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. إعداد الـ CORS - السماح لجميع العمليات (ضروري جداً للفرونت إيند)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 3. إعداد Identity للمستخدمين والأدوار
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // 4. إعداد الـ Controllers مع معالجة الـ JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            // 5. إعداد Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DEPI Project", Version = "v1" });
                c.CustomSchemaIds(type => type.FullName);
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            // 6. تسجيل الخدمات (Dependency Injection)
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // تسجيل خدمة التنبيهات الخلفية (Background Service)
            builder.Services.AddHostedService<AppointmentReminderService>();

            var app = builder.Build();

            // --- Middleware بالترتيب الصحيح لضمان استقرار الربط ---

            // يجب أن يكون UseCors أول شيء بعد Build مباشرة
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DEPI Project v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // للسماح بالوصول لصور الملف الشخصي

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // تنفيذ الـ Migrations تلقائياً عند تشغيل السيرفر
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            app.Run();
        }
    }
}