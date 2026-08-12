namespace ShiftFlow.Domain.ShiftAssignments;

/// <summary>
/// Puerto de persistencia de asignaciones de turno.
/// </summary>
public interface IShiftAssignmentRepository
{
    /// <summary>
    /// Obtiene una asignación por identificador.
    /// </summary>
    /// <param name="id">Identificador de la asignación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>La asignación o <c>null</c> si no existe.</returns>
    Task<ShiftAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista asignaciones <see cref="ShiftAssignmentStatus.Assigned"/> del empleado.
    /// </summary>
    /// <param name="employeeId">Empleado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Asignaciones vigentes ordenadas por inicio.</returns>
    Task<IReadOnlyList<ShiftAssignment>> ListAssignedByEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista asignaciones <see cref="ShiftAssignmentStatus.Assigned"/> de la organización
    /// cuyo intervalo intersecta el mes civil indicado.
    /// </summary>
    /// <param name="organizationId">Organización.</param>
    /// <param name="year">Año del calendario.</param>
    /// <param name="month">Mes (1–12).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Asignaciones del mes ordenadas por <c>StartAt</c>.</returns>
    Task<IReadOnlyList<ShiftAssignment>> ListAssignedIntersectingMonthAsync(
        Guid organizationId,
        int year,
        int month,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade una asignación nueva al almacén.
    /// </summary>
    /// <param name="assignment">Asignación a persistir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(ShiftAssignment assignment, CancellationToken cancellationToken = default);
}
