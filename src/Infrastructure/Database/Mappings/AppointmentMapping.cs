using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mappings;

public class AppointmentMapping : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable(nameof(Appointment));
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AppointmentDateTime).IsRequired();

        builder.HasOne(a => a.Doctor)
            .WithMany()
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Specialty)
            .WithMany()
            .HasForeignKey(a => a.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.AppointmentSpan)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.AppointmentSpanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<string>(a => a.Status).IsRequired().HasMaxLength(50);
    }
}