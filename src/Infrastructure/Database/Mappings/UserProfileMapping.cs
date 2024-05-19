using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mappings
{
    public class UserProfileMapping : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");

            // Configure the many-to-many relationship between UserProfile and MedicalSpecialty
            builder.HasMany(up => up.MedicalSpecialties)
                .WithMany(ms => ms.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserMedicalSpecialties",
                    j => j
                        .HasOne<MedicalSpecialty>()
                        .WithMany()
                        .HasForeignKey("MedicalSpecialtyId"),
                    j => j
                        .HasOne<UserProfile>()
                        .WithMany()
                        .HasForeignKey("UserProfileId"));
        }
    }
}