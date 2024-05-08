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
        builder.Property(ms => ms.IsActive).HasDefaultValue(true);

        // Many-to-many join configuration
        builder.HasMany(ms => ms.Users)
            .WithMany(u => u.MedicalSpecialties)
            .UsingEntity(j => j.ToTable("UserMedicalSpecialties"));
    }
}