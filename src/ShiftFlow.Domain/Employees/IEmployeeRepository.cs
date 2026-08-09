namespace ShiftFlow.Domain.Employees;

/// <summary>
/// Puerto de persistencia de empleados.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Obtiene un empleado por identificador.
    /// </summary>
    /// <param name="id">Identificador del empleado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El empleado o <c>null</c> si no existe.</returns>
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista los empleados de una organización.
    /// </summary>
    /// <param name="organizationId">Organización propietaria.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de empleados de la organización.</returns>
    Task<IReadOnlyList<Employee>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista los empleados de un departamento.
    /// </summary>
    /// <param name="departmentId">Departamento de asignación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de empleados del departamento.</returns>
    Task<IReadOnlyList<Employee>> ListByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Indica si ya existe un empleado con el mismo email en la organización.
    /// </summary>
    /// <param name="organizationId">Organización en la que buscar.</param>
    /// <param name="email">Email a comprobar.</param>
    /// <param name="excludingEmployeeId">Empleado a excluir (actualizaciones).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>true</c> si el email ya está en uso.</returns>
    Task<bool> ExistsWithEmailAsync(
        Guid organizationId,
        string email,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un empleado al almacén.
    /// </summary>
    /// <param name="employee">Agregado a persistir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
}
