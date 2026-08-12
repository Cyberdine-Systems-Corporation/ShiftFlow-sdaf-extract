using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Leaves;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Leaves;

/// <summary>
/// Consulta de leaves de una organización.
/// </summary>
/// <param name="OrganizationId">Organización.</param>
/// <param name="EmployeeId">Filtro opcional de empleado.</param>
/// <param name="Year">Año opcional (con <paramref name="Month"/>).</param>
/// <param name="Month">Mes opcional (1–12).</param>
/// <param name="ActiveOnly">Si es <c>true</c>, solo Status Active (por defecto).</param>
public sealed record ListLeavesQuery(
    Guid OrganizationId,
    Guid? EmployeeId = null,
    int? Year = null,
    int? Month = null,
    bool ActiveOnly = true) : IRequest<IReadOnlyList<LeaveDto>>;

/// <summary>
/// Handler de listado de leaves.
/// </summary>
public sealed class ListLeavesHandler(
    IOrganizationRepository organizations,
    ILeaveRepository leaves) : IRequestHandler<ListLeavesQuery, IReadOnlyList<LeaveDto>>
{
    /// <summary>
    /// Lista leaves del alcance pedido.
    /// </summary>
    /// <param name="request">Consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de DTOs.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Si el mes no está entre 1 y 12.</exception>
    public async Task<IReadOnlyList<LeaveDto>> Handle(
        ListLeavesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "El mes debe estar entre 1 y 12.");
        }

        if (request.Year.HasValue != request.Month.HasValue)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request),
                "Year y Month deben informarse juntos o ninguno.");
        }

        _ = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        IReadOnlyList<Leave>? items = await leaves.ListAsync(
            request.OrganizationId,
            request.EmployeeId,
            request.Year,
            request.Month,
            request.ActiveOnly,
            cancellationToken);

        return items.Select(RegisterLeaveHandler.ToDto).ToList();
    }
}
