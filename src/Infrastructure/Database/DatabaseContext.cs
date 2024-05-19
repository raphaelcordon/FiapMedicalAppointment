using Domain.Entities;
using Infrastructure.Database.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : IdentityDbContext<UserProfile, Role, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentSpan> AppointmentSpans { get; set; }
    public DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; } // Ensure this line is present

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AppointmentMapping());
        modelBuilder.ApplyConfiguration(new AppointmentSpanMapping());
        modelBuilder.ApplyConfiguration(new RoleMapping());
        modelBuilder.ApplyConfiguration(new MedicalSpecialtyMapping());
        modelBuilder.ApplyConfiguration(new UserProfileMapping());

        // Seed Appointment Spans
        modelBuilder.Entity<AppointmentSpan>().HasData(
            new AppointmentSpan { Id = Guid.NewGuid(), Duration = 15 },
            new AppointmentSpan { Id = Guid.NewGuid(), Duration = 30 },
            new AppointmentSpan { Id = Guid.NewGuid(), Duration = 45 },
            new AppointmentSpan { Id = Guid.NewGuid(), Duration = 60 }
        );
    }
}