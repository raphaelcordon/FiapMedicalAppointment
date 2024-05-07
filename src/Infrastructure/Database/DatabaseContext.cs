using Domain.Entities;
using Infrastructure.Database.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class DatabaseContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentSpan> AppointmentSpans { get; set; }
    public DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new AppointmentMapping());
        modelBuilder.ApplyConfiguration(new AppointmentSpanMapping());
        modelBuilder.ApplyConfiguration(new RoleMapping());
        modelBuilder.ApplyConfiguration(new MedicalSpecialtyMapping());
    }
}