using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftFlow.Domain.ShiftAssignments;

namespace ShiftFlow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapeo Fluent API de <see cref="ShiftAssignment"/>.
/// </summary>
public sealed class ShiftAssignmentConfiguration : IEntityTypeConfiguration<ShiftAssignment>
{
    /// <summary>
    /// Configura tabla, índices y propiedades de asignación de turno.
    /// </summary>
    /// <param name="builder">Constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<ShiftAssignment> builder)
    {
        builder.ToTable("ShiftAssignments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.ShiftTypeId).IsRequired();
        builder.Property(x => x.StartAt).IsRequired();
        builder.Property(x => x.EndAt).IsRequired();
        builder.Property(x => x.Status).IsRequired().HasConversion<int>();
        builder.HasIndex(x => x.OrganizationId);
        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => new { x.OrganizationId, x.StartAt });
    }
}
