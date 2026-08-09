namespace ShiftFlow.Domain.Employees;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Employee>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Employee>> ListByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsWithEmailAsync(
        Guid organizationId,
        string email,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
}
