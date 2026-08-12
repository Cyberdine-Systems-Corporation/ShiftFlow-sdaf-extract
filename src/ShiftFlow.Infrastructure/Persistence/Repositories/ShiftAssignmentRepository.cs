using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.ShiftAssignments;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Adaptador EF Core del puerto <see cref="IShiftAssignmentRepository"/>.
/// </summary>
public sealed class ShiftAssignmentRepository(ShiftFlowDbContext db) : IShiftAssignmentRepository
{
    #region Queries

    /// <inheritdoc />
    public Task<ShiftAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.ShiftAssignments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<ShiftAssignment>> ListAssignedByEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        // Filtro Status + OrderBy en memoria: SQLite de tests no traduce bien DateTimeOffset.
        var items = await db.ShiftAssignments
            .Where(x => x.EmployeeId == employeeId && x.Status == ShiftAssignmentStatus.Assigned)
            .ToListAsync(cancellationToken);

        return items.OrderBy(x => x.StartAt).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ShiftAssignment>> ListAssignedIntersectingMonthAsync(
        Guid organizationId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        // Intersección con [monthStart, nextMonthStart) evaluada en memoria (compat. SQLite + Npgsql).
        var monthStart = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
        var monthEnd = monthStart.AddMonths(1);

        var items = await db.ShiftAssignments
            .Where(x => x.OrganizationId == organizationId && x.Status == ShiftAssignmentStatus.Assigned)
            .ToListAsync(cancellationToken);

        return items
            .Where(x => x.StartAt < monthEnd && x.EndAt > monthStart)
            .OrderBy(x => x.StartAt)
            .ToList();
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public async Task AddAsync(ShiftAssignment assignment, CancellationToken cancellationToken = default) =>
        await db.ShiftAssignments.AddAsync(assignment, cancellationToken);

    #endregion
}
