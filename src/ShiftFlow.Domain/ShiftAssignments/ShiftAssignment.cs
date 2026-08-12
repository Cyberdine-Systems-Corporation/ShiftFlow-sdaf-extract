using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.ShiftAssignments;

/// <summary>
/// Agregado de turno asignado a un empleado en un intervalo temporal.
/// </summary>
public sealed class ShiftAssignment
{
    private ShiftAssignment()
    {
    }

    /// <summary>
    /// Identificador de la asignación.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Organización de planificación.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Empleado asignado.
    /// </summary>
    public Guid EmployeeId { get; private set; }

    /// <summary>
    /// Tipo de turno del catálogo.
    /// </summary>
    public Guid ShiftTypeId { get; private set; }

    /// <summary>
    /// Inicio del intervalo (semiabierto <c>[StartAt, EndAt)</c>).
    /// </summary>
    public DateTimeOffset StartAt { get; private set; }

    /// <summary>
    /// Fin exclusivo del intervalo.
    /// </summary>
    public DateTimeOffset EndAt { get; private set; }

    /// <summary>
    /// Estado de la asignación.
    /// </summary>
    public ShiftAssignmentStatus Status { get; private set; }

    #region Factory

    /// <summary>
    /// Crea una asignación <see cref="ShiftAssignmentStatus.Assigned"/> validando invariantes estructurales.
    /// </summary>
    /// <param name="organizationId">Organización de planificación.</param>
    /// <param name="employeeId">Empleado candidato.</param>
    /// <param name="employeeOrganizationId">Organización real del empleado (INV-ASN-01).</param>
    /// <param name="employeeIsActive">Estado del empleado (INV-ASN-02).</param>
    /// <param name="shiftTypeId">Tipo de turno candidato.</param>
    /// <param name="shiftTypeOrganizationId">Organización real del tipo (INV-ASN-01).</param>
    /// <param name="shiftTypeIsActive">Estado del tipo (INV-ASN-03).</param>
    /// <param name="startAt">Inicio del turno.</param>
    /// <param name="endAt">Fin del turno (debe ser &gt; <paramref name="startAt"/>).</param>
    /// <returns>Nueva asignación con identificador generado.</returns>
    public static ShiftAssignment Create(
        Guid organizationId,
        Guid employeeId,
        Guid employeeOrganizationId,
        bool employeeIsActive,
        Guid shiftTypeId,
        Guid shiftTypeOrganizationId,
        bool shiftTypeIsActive,
        DateTimeOffset startAt,
        DateTimeOffset endAt)
    {
        EnsureSameOrganization(organizationId, employeeId, employeeOrganizationId, shiftTypeId, shiftTypeOrganizationId);

        if (!employeeIsActive)
        {
            throw new DomainException("INV-ASN-02", "No se puede asignar un turno a un empleado inactivo.");
        }

        if (!shiftTypeIsActive)
        {
            throw new DomainException(
                "INV-ASN-03",
                "No se puede asignar un turno con un tipo de turno inactivo.");
        }

        EnsureValidInterval(startAt, endAt);

        return new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            EmployeeId = employeeId,
            ShiftTypeId = shiftTypeId,
            StartAt = startAt,
            EndAt = endAt,
            Status = ShiftAssignmentStatus.Assigned
        };
    }

    #endregion

    #region Behavior

    /// <summary>
    /// Cancela una asignación vigente (INV-ASN-06).
    /// </summary>
    public void Cancel()
    {
        if (Status != ShiftAssignmentStatus.Assigned)
        {
            throw new DomainException(
                "INV-ASN-06",
                "Solo se puede cancelar una asignación en estado Assigned.");
        }

        Status = ShiftAssignmentStatus.Cancelled;
    }

    #endregion

    #region Invariants

    private static void EnsureSameOrganization(
        Guid organizationId,
        Guid employeeId,
        Guid employeeOrganizationId,
        Guid shiftTypeId,
        Guid shiftTypeOrganizationId)
    {
        if (organizationId == Guid.Empty)
        {
            throw new DomainException("INV-ASN-01", "La asignación requiere una organización válida.");
        }

        if (employeeId == Guid.Empty || shiftTypeId == Guid.Empty)
        {
            throw new DomainException("INV-ASN-01", "La asignación requiere empleado y tipo de turno válidos.");
        }

        if (employeeOrganizationId != organizationId || shiftTypeOrganizationId != organizationId)
        {
            throw new DomainException(
                "INV-ASN-01",
                "El empleado y el tipo de turno deben pertenecer a la misma organización de planificación.");
        }
    }

    private static void EnsureValidInterval(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        // INV-ASN-04: sin overnight ni intervalos vacíos/invertidos.
        if (endAt <= startAt)
        {
            throw new DomainException(
                "INV-ASN-04",
                "El fin del turno debe ser posterior al inicio (sin cruce de medianoche en MVP).");
        }
    }

    #endregion
}
