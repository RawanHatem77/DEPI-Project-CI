    using DEPI.Infrastructure.Data;
    using DEPI.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting; 
    using Microsoft.Extensions.DependencyInjection;

    namespace DEPI.Infrastructure.Service
    {
        public class AppointmentReminderService : BackgroundService
        {
            private readonly IServiceProvider _serviceProvider;

            public AppointmentReminderService(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                  
                    var now = DateTime.Now;

                    // --- الجزء الأول: إرسال التنبيهات ---
                    var targetTimeForReminder = now.AddDays(1);
                    var upcomingAppointments = await context.Appointments
                        .Where(a => a.ReservationDate >= now
                                    && a.ReservationDate <= targetTimeForReminder
                                    && !a.IsReminderSent)
                        .ToListAsync();

                    foreach (var app in upcomingAppointments)
                    {
                        context.Notifications.Add(new Notification
                        {
                            PatientId = app.PatientId,
                            Title = "تذكير بموعد",
                            Message = $"نود تذكيرك بموعدك القادم غداً بتاريخ {app.ReservationDate.ToShortDateString()}",
                            CreatedAt = now, 
                            IsRead = false,
                            Type = "Reminder" 
                        });
                        app.IsReminderSent = true;
                    }

                    // --- الجزء الثاني: تحديث الحالة من Pending إلى Done ---
                    var pastAppointments = await context.Appointments
                        .Where(a => a.Status == "Pending" &&
                                    (a.ReservationDate.Date.Add(a.ReservationTime)) < now)
                        .ToListAsync();

                    foreach (var app in pastAppointments)
                    {
                        app.Status = "Done";
                    }

                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
    }