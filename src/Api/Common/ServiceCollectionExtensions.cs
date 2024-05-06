using Api.Services;
using Domain.Interfaces;
using Infrastructure.Database.Repositories;

namespace Api.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperConfigurations>());

        return services;
    }
}