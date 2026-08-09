using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Adaptador EF Core del puerto <see cref="IEmployeeRepository"/>.
/// </summary>
public sealed class EmployeeRepository(ShiftFlowDbContext db) : IEmployeeRepository
{
    #region Queries

    /// <inheritdoc />
    public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Employee>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default) =>
        await db.Employees
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.DisplayName)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Employee>> ListByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default) =>
        await db.Employees
            .Where(x => x.DepartmentId == departmentId)
            .OrderBy(x => x.DisplayName)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public Task<bool> ExistsWithEmailAsync(
        Guid organizationId,
        string email,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return db.Employees.AnyAsync(
            x => x.OrganizationId == organizationId
                 && x.Email != null
                 && x.Email.ToLower() == normalized
                 && (excludingEmployeeId == null || x.Id != excludingEmployeeId),
            cancellationToken);
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default) =>
        await db.Employees.AddAsync(employee, cancellationToken);

    #endregion
}
