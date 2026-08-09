namespace ShiftFlow.Domain.Departments;

/// <summary>
/// Puerto de persistencia de departamentos.
/// </summary>
public interface IDepartmentRepository
{
    /// <summary>
    /// Obtiene un departamento por identificador.
    /// </summary>
    /// <param name="id">Identificador del departamento.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El departamento o <c>null</c> si no existe.</returns>
    Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista los departamentos de una organización.
    /// </summary>
    /// <param name="organizationId">Organización propietaria.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de departamentos de la organización.</returns>
    Task<IReadOnlyList<Department>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Indica si ya existe un departamento con el mismo nombre en la organización.
    /// </summary>
    /// <param name="organizationId">Organización en la que buscar.</param>
    /// <param name="name">Nombre a comprobar.</param>
    /// <param name="excludingDepartmentId">Departamento a excluir (actualizaciones).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>true</c> si el nombre ya está en uso.</returns>
    Task<bool> ExistsWithNameAsync(
        Guid organizationId,
        string name,
        Guid? excludingDepartmentId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un departamento al almacén.
    /// </summary>
    /// <param name="department">Agregado a persistir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(Department department, CancellationToken cancellationToken = default);
}
