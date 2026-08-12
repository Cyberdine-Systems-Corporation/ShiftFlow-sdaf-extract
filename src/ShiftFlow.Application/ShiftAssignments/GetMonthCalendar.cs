using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Leaves;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftAssignments;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftAssignments;

/// <summary>
/// Consulta del calendario mensual de una organización.
/// </summary>
/// <param name="OrganizationId">Organización.</param>
/// <param name="Year">Año (gregoriano).</param>
/// <param name="Month">Mes (1–12).</param>
public sealed record GetMonthCalendarQuery(Guid OrganizationId, int Year, int Month)
    : IRequest<MonthCalendarDto>;

/// <summary>
/// Proyección mensual: asignaciones Assigned y leaves Active (SPEC-DOM-005/007).
/// </summary>
/// <param name="Assignments">Turnos Assigned que intersectan el mes.</param>
/// <param name="Leaves">Ausencias Active que intersectan el mes.</param>
public sealed record MonthCalendarDto(
    IReadOnlyList<CalendarAssignmentDto> Assignments,
    IReadOnlyList<CalendarLeaveDto> Leaves);

/// <summary>
/// Entrada de calendario con metadatos mínimos de empleado y tipo.
/// </summary>
/// <param name="Id">Identificador de la asignación.</param>
/// <param name="EmployeeId">Empleado.</param>
/// <param name="EmployeeDisplayName">Nombre visible del empleado.</param>
/// <param name="ShiftTypeId">Tipo de turno.</param>
/// <param name="ShiftTypeName">Nombre del tipo de turno.</param>
/// <param name="StartAt">Inicio.</param>
/// <param name="EndAt">Fin.</param>
public sealed record CalendarAssignmentDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeDisplayName,
    Guid ShiftTypeId,
    string ShiftTypeName,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt);

/// <summary>
/// Ausencia proyectada en el calendario mensual.
/// </summary>
/// <param name="Id">Identificador del leave.</param>
/// <param name="EmployeeId">Empleado.</param>
/// <param name="EmployeeDisplayName">Nombre visible del empleado.</param>
/// <param name="StartOn">Inicio inclusive.</param>
/// <param name="EndOn">Fin inclusive.</param>
/// <param name="Kind">Tipo opcional.</param>
public sealed record CalendarLeaveDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeDisplayName,
    DateOnly StartOn,
    DateOnly EndOn,
    string? Kind);

/// <summary>
/// Handler de proyección de calendario mensual (asignaciones + leaves).
/// </summary>
public sealed class GetMonthCalendarHandler(
    IOrganizationRepository organizations,
    IShiftAssignmentRepository assignments,
    ILeaveRepository leaves,
    IEmployeeRepository employees,
    IShiftTypeRepository shiftTypes)
    : IRequestHandler<GetMonthCalendarQuery, MonthCalendarDto>
{
    /// <summary>
    /// Obtiene asignaciones Assigned y leaves Active que intersectan el mes.
    /// </summary>
    /// <param name="request">Consulta de calendario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Proyección mensual.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Si el mes no está entre 1 y 12.</exception>
    public async Task<MonthCalendarDto> Handle(
        GetMonthCalendarQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "El mes debe estar entre 1 y 12.");
        }

        _ = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        IReadOnlyList<ShiftAssignment>? monthAssignments = await assignments.ListAssignedIntersectingMonthAsync(
            request.OrganizationId,
            request.Year,
            request.Month,
            cancellationToken);

        IReadOnlyList<Leave>? monthLeaves = await leaves.ListAsync(
            request.OrganizationId,
            employeeId: null,
            request.Year,
            request.Month,
            activeOnly: true,
            cancellationToken);

        IReadOnlyList<Employee>? orgEmployees = await employees.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        Dictionary<Guid, string> employeeNames = orgEmployees.ToDictionary(e => e.Id, e => e.DisplayName);

        IReadOnlyList<CalendarAssignmentDto> assignmentsDto;
        if (monthAssignments.Count == 0)
        {
            assignmentsDto = Array.Empty<CalendarAssignmentDto>();
        }
        else
        {
            IReadOnlyList<ShiftType>? orgShiftTypes = await shiftTypes.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
            Dictionary<Guid, string> shiftTypeNames = orgShiftTypes.ToDictionary(s => s.Id, s => s.Name);
            assignmentsDto = monthAssignments
                .Select(a => new CalendarAssignmentDto(
                    a.Id,
                    a.EmployeeId,
                    employeeNames.GetValueOrDefault(a.EmployeeId, string.Empty),
                    a.ShiftTypeId,
                    shiftTypeNames.GetValueOrDefault(a.ShiftTypeId, string.Empty),
                    a.StartAt,
                    a.EndAt))
                .ToList();
        }

        List<CalendarLeaveDto> leavesDto = monthLeaves
            .Select(l => new CalendarLeaveDto(
                l.Id,
                l.EmployeeId,
                employeeNames.GetValueOrDefault(l.EmployeeId, string.Empty),
                l.StartOn,
                l.EndOn,
                l.Kind))
            .ToList();

        return new MonthCalendarDto(assignmentsDto, leavesDto);
    }
}
