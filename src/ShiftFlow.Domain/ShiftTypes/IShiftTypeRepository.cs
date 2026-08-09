namespace ShiftFlow.Domain.ShiftTypes;

public interface IShiftTypeRepository
{
    Task<ShiftType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShiftType>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsWithNameAsync(
        Guid organizationId,
        string name,
        Guid? excludingShiftTypeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsWithCodeAsync(
        Guid organizationId,
        string code,
        Guid? excludingShiftTypeId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(ShiftType shiftType, CancellationToken cancellationToken = default);
}
