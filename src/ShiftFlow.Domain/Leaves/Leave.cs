using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Leaves;

/// <summary>
/// Agregado Leave: ausencia/vacación que bloquea nuevas asignaciones (HR-02).
/// </summary>
public sealed class Leave
{
    /// <summary>
    /// Longitud máxima del motivo opcional.
    /// </summary>
    public const int ReasonMaxLength = 500;

    private Leave()
    {
    }

    /// <summary>
    /// Identificador del leave.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Organización de planificación.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Empleado ausente.
    /// </summary>
    public Guid EmployeeId { get; private set; }

    /// <summary>
    /// Fecha civil de inicio (inclusive).
    /// </summary>
    public DateOnly StartOn { get; private set; }

    /// <summary>
    /// Fecha civil de fin (inclusive).
    /// </summary>
    public DateOnly EndOn { get; private set; }

    /// <summary>
    /// Estado del leave.
    /// </summary>
    public LeaveStatus Status { get; private set; }

    /// <summary>
    /// Tipo opcional (<c>Vacation</c>, <c>Other</c>, …); no afecta HR-02.
    /// </summary>
    public string? Kind { get; private set; }

    /// <summary>
    /// Motivo opcional corto.
    /// </summary>
    public string? Reason { get; private set; }

    #region Factory

    /// <summary>
    /// Registra un leave <see cref="LeaveStatus.Active"/> validando invariantes estructurales.
    /// </summary>
    /// <param name="organizationId">Organización de planificación.</param>
    /// <param name="employeeId">Empleado candidato.</param>
    /// <param name="employeeOrganizationId">Organización real del empleado (INV-LEA-01).</param>
    /// <param name="employeeIsActive">Estado del empleado (INV-LEA-02).</param>
    /// <param name="startOn">Inicio inclusive.</param>
    /// <param name="endOn">Fin inclusive (debe ser ≥ <paramref name="startOn"/>).</param>
    /// <param name="kind">Tipo opcional.</param>
    /// <param name="reason">Motivo opcional.</param>
    /// <returns>Nuevo leave con identificador generado.</returns>
    public static Leave Create(
        Guid organizationId,
        Guid employeeId,
        Guid employeeOrganizationId,
        bool employeeIsActive,
        DateOnly startOn,
        DateOnly endOn,
        string? kind = null,
        string? reason = null)
    {
        EnsureSameOrganization(organizationId, employeeId, employeeOrganizationId);

        if (!employeeIsActive)
        {
            throw new DomainException("INV-LEA-02", "No se puede registrar una ausencia para un empleado inactivo.");
        }

        EnsureValidDateRange(startOn, endOn);

        return new Leave
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            EmployeeId = employeeId,
            StartOn = startOn,
            EndOn = endOn,
            Status = LeaveStatus.Active,
            Kind = NormalizeOptional(kind, maxLength: 50),
            Reason = NormalizeOptional(reason, ReasonMaxLength)
        };
    }

    #endregion

    #region Behavior

    /// <summary>
    /// Cancela un leave vigente (INV-LEA-05).
    /// </summary>
    public void Cancel()
    {
        if (Status != LeaveStatus.Active)
        {
            throw new DomainException(
                "INV-LEA-05",
                "Solo se puede cancelar una ausencia en estado Active.");
        }

        Status = LeaveStatus.Cancelled;
    }

    /// <summary>
    /// Cobertura temporal semiabierta <c>[StartOn 00:00, EndOn+1d 00:00)</c> en el offset indicado.
    /// </summary>
    /// <param name="offset">Offset del reloj homogéneo del runtime (MVP).</param>
    /// <returns>Par inicio/fin exclusivo para comparar con intervalos de turno.</returns>
    public (DateTimeOffset CoverageStart, DateTimeOffset CoverageEndExclusive) GetCoverage(TimeSpan offset)
    {
        DateTimeOffset coverageStart = new DateTimeOffset(StartOn.ToDateTime(TimeOnly.MinValue), offset);
        DateTimeOffset coverageEnd = new DateTimeOffset(EndOn.AddDays(1).ToDateTime(TimeOnly.MinValue), offset);
        return (coverageStart, coverageEnd);
    }

    /// <summary>
    /// Indica si la cobertura del leave intersecta el intervalo semiabierto del candidato.
    /// </summary>
    /// <param name="candidateStart">Inicio del turno candidato.</param>
    /// <param name="candidateEnd">Fin exclusivo del turno candidato.</param>
    /// <returns><c>true</c> si hay intersección no vacía.</returns>
    public bool CoversInterval(DateTimeOffset candidateStart, DateTimeOffset candidateEnd)
    {
        // Usa el offset del candidato para alinear cobertura civil y turno en el mismo reloj.
        (DateTimeOffset coverageStart, DateTimeOffset coverageEnd) = GetCoverage(candidateStart.Offset);
        return candidateStart < coverageEnd && coverageStart < candidateEnd;
    }

    #endregion

    #region Invariants

    private static void EnsureSameOrganization(
        Guid organizationId,
        Guid employeeId,
        Guid employeeOrganizationId)
    {
        if (organizationId == Guid.Empty)
        {
            throw new DomainException("INV-LEA-01", "La ausencia requiere una organización válida.");
        }

        if (employeeId == Guid.Empty)
        {
            throw new DomainException("INV-LEA-01", "La ausencia requiere un empleado válido.");
        }

        if (employeeOrganizationId != organizationId)
        {
            throw new DomainException(
                "INV-LEA-01",
                "El empleado debe pertenecer a la misma organización de planificación.");
        }
    }

    private static void EnsureValidDateRange(DateOnly startOn, DateOnly endOn)
    {
        if (endOn < startOn)
        {
            throw new DomainException(
                "INV-LEA-03",
                "La fecha de fin de la ausencia debe ser posterior o igual a la de inicio.");
        }
    }

    private static string? NormalizeOptional(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new DomainException(
                "INV-LEA-01",
                $"El texto opcional no puede superar {maxLength} caracteres.");
        }

        return trimmed;
    }

    #endregion
}
