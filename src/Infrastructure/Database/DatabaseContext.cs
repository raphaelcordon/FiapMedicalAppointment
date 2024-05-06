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
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<AppointmentSpan> AppointmentSpans { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new AppointmentMapping());
        modelBuilder.ApplyConfiguration(new AppointmentSpanMapping());
        modelBuilder.ApplyConfiguration(new DoctorMapping());
        modelBuilder.ApplyConfiguration(new PatientMapping());
        modelBuilder.ApplyConfiguration(new RoleMapping());
    }
}