using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftFlow.Domain.Departments;

namespace ShiftFlow.Infrastructure.Persistence.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Department.NameMaxLength);
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => new { x.OrganizationId, x.Name });
        builder.HasIndex(x => x.OrganizationId);
    }
}
