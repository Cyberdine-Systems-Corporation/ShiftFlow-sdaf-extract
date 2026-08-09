using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Infrastructure.Persistence.Configurations;

public sealed class ShiftTypeConfiguration : IEntityTypeConfiguration<ShiftType>
{
    public void Configure(EntityTypeBuilder<ShiftType> builder)
    {
        builder.ToTable("ShiftTypes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ShiftType.NameMaxLength);
        builder.Property(x => x.Code).HasMaxLength(ShiftType.CodeMaxLength);
        builder.Property(x => x.DefaultStartTime);
        builder.Property(x => x.DefaultEndTime);
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => x.OrganizationId);
        builder.HasIndex(x => new { x.OrganizationId, x.Name });
        builder.HasIndex(x => new { x.OrganizationId, x.Code });
    }
}
