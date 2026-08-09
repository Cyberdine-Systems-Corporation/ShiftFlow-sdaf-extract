using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Adaptador EF Core del puerto <see cref="IDepartmentRepository"/>.
/// </summary>
public sealed class DepartmentRepository(ShiftFlowDbContext db) : IDepartmentRepository
{
    #region Queries

    /// <inheritdoc />
    public Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Departments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Department>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default) =>
        await db.Departments
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public Task<bool> ExistsWithNameAsync(
        Guid organizationId,
        string name,
        Guid? excludingDepartmentId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLowerInvariant();
        return db.Departments.AnyAsync(
            x => x.OrganizationId == organizationId
                 && x.Name.ToLower() == normalized
                 && (excludingDepartmentId == null || x.Id != excludingDepartmentId),
            cancellationToken);
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public async Task AddAsync(Department department, CancellationToken cancellationToken = default) =>
        await db.Departments.AddAsync(department, cancellationToken);

    #endregion
}
