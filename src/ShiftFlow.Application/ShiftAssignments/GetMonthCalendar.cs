using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Employees;
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
    : IRequest<IReadOnlyList<CalendarAssignmentDto>>;

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
/// Handler de proyección de calendario mensual (solo Status Assigned).
/// </summary>
public sealed class GetMonthCalendarHandler(
    IOrganizationRepository organizations,
    IShiftAssignmentRepository assignments,
    IEmployeeRepository employees,
    IShiftTypeRepository shiftTypes)
    : IRequestHandler<GetMonthCalendarQuery, IReadOnlyList<CalendarAssignmentDto>>
{
    /// <summary>
    /// Obtiene las asignaciones Assigned que intersectan el mes.
    /// </summary>
    /// <param name="request">Consulta de calendario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista ordenada por inicio.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Si el mes no está entre 1 y 12.</exception>
    public async Task<IReadOnlyList<CalendarAssignmentDto>> Handle(
        GetMonthCalendarQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "El mes debe estar entre 1 y 12.");
        }

        _ = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        var monthAssignments = await assignments.ListAssignedIntersectingMonthAsync(
            request.OrganizationId,
            request.Year,
            request.Month,
            cancellationToken);

        if (monthAssignments.Count == 0)
        {
            return Array.Empty<CalendarAssignmentDto>();
        }

        var orgEmployees = await employees.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        var orgShiftTypes = await shiftTypes.ListByOrganizationAsync(request.OrganizationId, cancellationToken);

        var employeeNames = orgEmployees.ToDictionary(e => e.Id, e => e.DisplayName);
        var shiftTypeNames = orgShiftTypes.ToDictionary(s => s.Id, s => s.Name);

        return monthAssignments
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
}
