using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapeo Fluent API de <see cref="Organization"/>.
/// </summary>
public sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    /// <summary>
    /// Configura tabla, clave y propiedades de organización.
    /// </summary>
    /// <param name="builder">Constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Organization.NameMaxLength);
        builder.Property(x => x.IsActive).IsRequired();
    }
}
