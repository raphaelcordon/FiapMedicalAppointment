using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mappings;

public class MedicalSpecialtyMapping : IEntityTypeConfiguration<MedicalSpecialty>
{
    public void Configure(EntityTypeBuilder<MedicalSpecialty> builder)
    {
        builder.ToTable("MedicalSpecialties");

        builder.HasKey(ms => ms.Id);
        builder.Property(ms => ms.Specialty).IsRequired();

        builder.HasMany(ms => ms.Users)
            .WithMany(u => u.MedicalSpecialties)
            .UsingEntity(j => j.ToTable("UserProfileMedicalSpecialties"));
    }
}