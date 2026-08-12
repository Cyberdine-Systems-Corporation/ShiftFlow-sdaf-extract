namespace ShiftFlow.Domain.Leaves;

/// <summary>
/// Puerto de persistencia de ausencias (Leave).
/// </summary>
public interface ILeaveRepository
{
    /// <summary>
    /// Obtiene un leave por identificador.
    /// </summary>
    /// <param name="id">Identificador.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El leave o <c>null</c> si no existe.</returns>
    Task<Leave?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista leaves <see cref="LeaveStatus.Active"/> del empleado.
    /// </summary>
    /// <param name="employeeId">Empleado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Leaves activos ordenados por <c>StartOn</c>.</returns>
    Task<IReadOnlyList<Leave>> ListActiveByEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista leaves de la organización, opcionalmente filtrados por empleado y mes.
    /// </summary>
    /// <param name="organizationId">Organización.</param>
    /// <param name="employeeId">Filtro opcional de empleado.</param>
    /// <param name="year">Año opcional (con <paramref name="month"/>).</param>
    /// <param name="month">Mes opcional (1–12).</param>
    /// <param name="activeOnly">Si es <c>true</c>, solo Status Active.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Leaves del alcance pedido ordenados por inicio.</returns>
    Task<IReadOnlyList<Leave>> ListAsync(
        Guid organizationId,
        Guid? employeeId = null,
        int? year = null,
        int? month = null,
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un leave nuevo al almacén.
    /// </summary>
    /// <param name="leave">Leave a persistir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(Leave leave, CancellationToken cancellationToken = default);
}
