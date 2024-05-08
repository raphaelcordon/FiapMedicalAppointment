using Api.Services;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Infrastructure.Database.Repositories;

namespace Api.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // Register services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMedicalSpecialtyService, MedicalSpecialtyService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        
        services.AddTransient<UserRolesResolver>();
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperConfigurations>());

        return services;
    }
}