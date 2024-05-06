using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mappings;

public class AppointmentSpanMapping : IEntityTypeConfiguration<AppointmentSpan>
{
    public void Configure(EntityTypeBuilder<AppointmentSpan> builder)
    {
        builder.ToTable(nameof(AppointmentSpan));

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Duration).IsRequired();
    }
}