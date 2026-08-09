using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Adaptador EF Core del puerto <see cref="IOrganizationRepository"/>.
/// </summary>
public sealed class OrganizationRepository(ShiftFlowDbContext db) : IOrganizationRepository
{
    #region Queries

    /// <inheritdoc />
    public Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Organizations.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Organization>> ListAsync(CancellationToken cancellationToken = default) =>
        await db.Organizations
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    #endregion

    #region Commands

    /// <inheritdoc />
    public async Task AddAsync(Organization organization, CancellationToken cancellationToken = default) =>
        await db.Organizations.AddAsync(organization, cancellationToken);

    #endregion
}
