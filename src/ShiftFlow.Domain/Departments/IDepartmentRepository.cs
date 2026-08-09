namespace ShiftFlow.Domain.Departments;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Department>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsWithNameAsync(
        Guid organizationId,
        string name,
        Guid? excludingDepartmentId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(Department department, CancellationToken cancellationToken = default);
}
