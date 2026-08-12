using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.Rules;
using ShiftFlow.Domain.ShiftAssignments;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftAssignments;

/// <summary>
/// Comando de asignación manual de un turno (Scheduling Engine).
/// </summary>
/// <param name="OrganizationId">Organización de planificación.</param>
/// <param name="EmployeeId">Empleado destino.</param>
/// <param name="ShiftTypeId">Tipo de turno del catálogo.</param>
/// <param name="StartAt">Inicio del intervalo.</param>
/// <param name="EndAt">Fin del intervalo (debe ser posterior a <paramref name="StartAt"/>).</param>
public sealed record AssignShiftCommand(
    Guid OrganizationId,
    Guid EmployeeId,
    Guid ShiftTypeId,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt) : IRequest<ShiftAssignmentDto>;

/// <summary>
/// DTO de lectura de una asignación de turno.
/// </summary>
/// <param name="Id">Identificador de la asignación.</param>
/// <param name="OrganizationId">Organización.</param>
/// <param name="EmployeeId">Empleado.</param>
/// <param name="ShiftTypeId">Tipo de turno.</param>
/// <param name="StartAt">Inicio.</param>
/// <param name="EndAt">Fin.</param>
/// <param name="Status">Estado (<c>Assigned</c> / <c>Cancelled</c>).</param>
public sealed record ShiftAssignmentDto(
    Guid Id,
    Guid OrganizationId,
    Guid EmployeeId,
    Guid ShiftTypeId,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    string Status);

/// <summary>
/// Handler que crea la asignación tras invariantes estructurales y Rule Engine (HR-01).
/// </summary>
public sealed class AssignShiftHandler(
    IOrganizationRepository organizations,
    IEmployeeRepository employees,
    IShiftTypeRepository shiftTypes,
    IShiftAssignmentRepository assignments,
    IUnitOfWork unitOfWork) : IRequestHandler<AssignShiftCommand, ShiftAssignmentDto>
{
    private readonly RuleEngine _ruleEngine = new();

    /// <summary>
    /// Ejecuta la asignación manual.
    /// </summary>
    /// <param name="request">Comando de asignación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO de la asignación creada.</returns>
    /// <exception cref="NotFoundException">Si falta organización, empleado o tipo.</exception>
    /// <exception cref="DomainException">Si falla invariante estructural o hard rule.</exception>
    public async Task<ShiftAssignmentDto> Handle(
        AssignShiftCommand request,
        CancellationToken cancellationToken)
    {
        _ = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        var employee = await employees.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"Empleado {request.EmployeeId} no encontrado.");

        var shiftType = await shiftTypes.GetByIdAsync(request.ShiftTypeId, cancellationToken)
            ?? throw new NotFoundException($"Tipo de turno {request.ShiftTypeId} no encontrado.");

        var candidate = ShiftAssignment.Create(
            request.OrganizationId,
            employee.Id,
            employee.OrganizationId,
            employee.IsActive,
            shiftType.Id,
            shiftType.OrganizationId,
            shiftType.IsActive,
            request.StartAt,
            request.EndAt);

        // ADR-003: Evaluate antes de persistir.
        var existing = await assignments.ListAssignedByEmployeeAsync(employee.Id, cancellationToken);
        var violations = _ruleEngine.Evaluate(candidate, existing);
        if (violations.Count > 0)
        {
            var first = violations[0];
            throw new DomainException(first.Code, first.Message);
        }

        await assignments.AddAsync(candidate, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(candidate);
    }

    /// <summary>
    /// Mapea el agregado a DTO de aplicación.
    /// </summary>
    /// <param name="assignment">Agregado de dominio.</param>
    /// <returns>DTO equivalente.</returns>
    internal static ShiftAssignmentDto ToDto(ShiftAssignment assignment) =>
        new(
            assignment.Id,
            assignment.OrganizationId,
            assignment.EmployeeId,
            assignment.ShiftTypeId,
            assignment.StartAt,
            assignment.EndAt,
            assignment.Status.ToString());
}
