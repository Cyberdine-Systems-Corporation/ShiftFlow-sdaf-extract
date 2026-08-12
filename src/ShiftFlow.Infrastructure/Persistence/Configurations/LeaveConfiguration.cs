using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftFlow.Domain.Leaves;

namespace ShiftFlow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapeo Fluent API de <see cref="Leave"/>.
/// </summary>
public sealed class LeaveConfiguration : IEntityTypeConfiguration<Leave>
{
    /// <summary>
    /// Configura tabla, índices y propiedades de leave.
    /// </summary>
    /// <param name="builder">Constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<Leave> builder)
    {
        builder.ToTable("Leaves");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.StartOn).IsRequired();
        builder.Property(x => x.EndOn).IsRequired();
        builder.Property(x => x.Status).IsRequired().HasConversion<int>();
        builder.Property(x => x.Kind).HasMaxLength(50);
        builder.Property(x => x.Reason).HasMaxLength(Leave.ReasonMaxLength);
        builder.HasIndex(x => x.OrganizationId);
        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => new { x.OrganizationId, x.StartOn });
    }
}
