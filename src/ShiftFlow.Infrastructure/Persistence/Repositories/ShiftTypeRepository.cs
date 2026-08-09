using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.ShiftTypes;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Adaptador EF Core del puerto <see cref="IShiftTypeRepository"/>.
/// </summary>
public sealed class ShiftTypeRepository(ShiftFlowDbContext db) : IShiftTypeRepository
{
    #region Queries

    /// <inheritdoc />
    public Task<ShiftType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.ShiftTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<ShiftType>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default) =>
        await db.ShiftTypes
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public Task<bool> ExistsWithNameAsync(
        Guid organizationId,
        string name,
        Guid? excludingShiftTypeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLowerInvariant();
        return db.ShiftTypes.AnyAsync(
            x => x.OrganizationId == organizationId
                 && x.Name.ToLower() == normalized
                 && (excludingShiftTypeId == null || x.Id != excludingShiftTypeId),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> ExistsWithCodeAsync(
        Guid organizationId,
        string code,
        Guid? excludingShiftTypeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToLowerInvariant();
        return db.ShiftTypes.AnyAsync(
            x => x.OrganizationId == organizationId
                 && x.Code != null
                 && x.Code.ToLower() == normalized
                 && (excludingShiftTypeId == null || x.Id != excludingShiftTypeId),
            cancellationToken);
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public async Task AddAsync(ShiftType shiftType, CancellationToken cancellationToken = default) =>
        await db.ShiftTypes.AddAsync(shiftType, cancellationToken);

    #endregion
}
