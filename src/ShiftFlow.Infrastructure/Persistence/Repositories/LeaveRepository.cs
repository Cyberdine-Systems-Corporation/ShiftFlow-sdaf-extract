using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.Leaves;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Adaptador EF Core del puerto <see cref="ILeaveRepository"/>.
/// </summary>
public sealed class LeaveRepository(ShiftFlowDbContext db) : ILeaveRepository
{
    #region Queries

    /// <inheritdoc />
    public Task<Leave?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Leaves.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Leave>> ListActiveByEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        List<Leave>? items = await db.Leaves
            .Where(x => x.EmployeeId == employeeId && x.Status == LeaveStatus.Active)
            .ToListAsync(cancellationToken);

        return items.OrderBy(x => x.StartOn).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Leave>> ListAsync(
        Guid organizationId,
        Guid? employeeId = null,
        int? year = null,
        int? month = null,
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Leave>? query = db.Leaves.Where(x => x.OrganizationId == organizationId);

        if (employeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == employeeId.Value);
        }

        if (activeOnly)
        {
            query = query.Where(x => x.Status == LeaveStatus.Active);
        }

        List<Leave>? items = await query.ToListAsync(cancellationToken);

        if (year.HasValue && month.HasValue)
        {
            // Intersección de fechas civiles con el mes: StartOn <= lastDay && EndOn >= firstDay.
            DateOnly firstDay = new DateOnly(year.Value, month.Value, 1);
            DateOnly lastDay = firstDay.AddMonths(1).AddDays(-1);
            items = items
                .Where(x => x.StartOn <= lastDay && x.EndOn >= firstDay)
                .ToList();
        }

        return items.OrderBy(x => x.StartOn).ThenBy(x => x.EndOn).ToList();
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public async Task AddAsync(Leave leave, CancellationToken cancellationToken = default) =>
        await db.Leaves.AddAsync(leave, cancellationToken);

    #endregion
}
