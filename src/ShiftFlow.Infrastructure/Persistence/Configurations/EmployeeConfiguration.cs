using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftFlow.Domain.Employees;

namespace ShiftFlow.Infrastructure.Persistence.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.Property(x => x.DepartmentId).IsRequired();
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(Employee.DisplayNameMaxLength);
        builder.Property(x => x.Email).HasMaxLength(Employee.EmailMaxLength);
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => x.OrganizationId);
        builder.HasIndex(x => x.DepartmentId);
        builder.HasIndex(x => new { x.OrganizationId, x.Email });
    }
}
