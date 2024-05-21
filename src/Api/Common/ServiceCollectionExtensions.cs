using Api.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Infrastructure.BackgroundServices;
using Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IBaseRepository<UserProfile>, UserRepository>();

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMedicalSpecialtyService, MedicalSpecialtyService>();
            services.AddHostedService<AppointmentReminderService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<RoleManager<Role>>();
            services.AddScoped<IRoleStore<Role>, RoleStore<Role, DatabaseContext, Guid>>();
            
            services.AddTransient<UserRolesResolver>();
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperConfigurations>());

            return services;
        }
    }
}