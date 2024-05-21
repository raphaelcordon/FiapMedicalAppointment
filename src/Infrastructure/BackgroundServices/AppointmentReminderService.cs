using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Dtos;
using Domain.Interfaces.Services;

namespace Infrastructure.BackgroundServices
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly ILogger<AppointmentReminderService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AppointmentReminderService(ILogger<AppointmentReminderService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendAppointmentRemindersAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                // await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run once a day
            }
        }

        private async Task SendAppointmentRemindersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                var tomorrow = DateTime.Now.AddDays(1).Date;
                var appointments = await appointmentService.GetAppointmentsByDateAsync(tomorrow);

                foreach (var appointment in appointments)
                {
                    emailSender.AppointmentEmailSender(appointment);
                }

                _logger.LogInformation("Appointment reminders sent successfully.");
            }
        }
    }
}