using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Leaves;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Leaves;

/// <summary>
/// Comando para registrar una ausencia activa.
/// </summary>
/// <param name="OrganizationId">Organización de planificación.</param>
/// <param name="EmployeeId">Empleado ausente.</param>
/// <param name="StartOn">Inicio inclusive.</param>
/// <param name="EndOn">Fin inclusive.</param>
/// <param name="Kind">Tipo opcional.</param>
/// <param name="Reason">Motivo opcional.</param>
public sealed record RegisterLeaveCommand(
    Guid OrganizationId,
    Guid EmployeeId,
    DateOnly StartOn,
    DateOnly EndOn,
    string? Kind = null,
    string? Reason = null) : IRequest<LeaveDto>;

/// <summary>
/// DTO de lectura de un leave.
/// </summary>
/// <param name="Id">Identificador.</param>
/// <param name="OrganizationId">Organización.</param>
/// <param name="EmployeeId">Empleado.</param>
/// <param name="StartOn">Inicio inclusive.</param>
/// <param name="EndOn">Fin inclusive.</param>
/// <param name="Status">Estado (<c>Active</c> / <c>Cancelled</c>).</param>
/// <param name="Kind">Tipo opcional.</param>
/// <param name="Reason">Motivo opcional.</param>
public sealed record LeaveDto(
    Guid Id,
    Guid OrganizationId,
    Guid EmployeeId,
    DateOnly StartOn,
    DateOnly EndOn,
    string Status,
    string? Kind,
    string? Reason);

/// <summary>
/// Handler de <see cref="RegisterLeaveCommand"/>.
/// </summary>
public sealed class RegisterLeaveHandler(
    IOrganizationRepository organizations,
    IEmployeeRepository employees,
    ILeaveRepository leaves,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterLeaveCommand, LeaveDto>
{
    /// <summary>
    /// Registra el leave Active.
    /// </summary>
    /// <param name="request">Comando.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del leave creado.</returns>
    /// <exception cref="NotFoundException">Si falta organización o empleado.</exception>
    /// <exception cref="DomainException">Si falla invariante estructural.</exception>
    public async Task<LeaveDto> Handle(RegisterLeaveCommand request, CancellationToken cancellationToken)
    {
        _ = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        Employee? employee = await employees.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"Empleado {request.EmployeeId} no encontrado.");

        Leave leave = Leave.Create(
            request.OrganizationId,
            employee.Id,
            employee.OrganizationId,
            employee.IsActive,
            request.StartOn,
            request.EndOn,
            request.Kind,
            request.Reason);

        await leaves.AddAsync(leave, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(leave);
    }

    /// <summary>
    /// Mapea el agregado a DTO.
    /// </summary>
    /// <param name="leave">Agregado.</param>
    /// <returns>DTO equivalente.</returns>
    internal static LeaveDto ToDto(Leave leave) =>
        new(
            leave.Id,
            leave.OrganizationId,
            leave.EmployeeId,
            leave.StartOn,
            leave.EndOn,
            leave.Status.ToString(),
            leave.Kind,
            leave.Reason);
}
