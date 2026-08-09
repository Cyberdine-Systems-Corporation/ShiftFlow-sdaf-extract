using MediatR;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftTypes;

/// <summary>
/// Lista los tipos de turno de una organización.
/// </summary>
/// <param name="OrganizationId">Identificador de la organización.</param>
public sealed record ListShiftTypesByOrganizationQuery(Guid OrganizationId)
    : IRequest<IReadOnlyList<ShiftTypeDto>>;

/// <summary>
/// Handler de <see cref="ListShiftTypesByOrganizationQuery"/>.
/// </summary>
public sealed class ListShiftTypesByOrganizationHandler(IShiftTypeRepository shiftTypes)
    : IRequestHandler<ListShiftTypesByOrganizationQuery, IReadOnlyList<ShiftTypeDto>>
{
    /// <summary>
    /// Devuelve los tipos de turno de la organización indicada.
    /// </summary>
    /// <param name="request">Consulta con el identificador de organización.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de DTOs de tipo de turno.</returns>
    public async Task<IReadOnlyList<ShiftTypeDto>> Handle(
        ListShiftTypesByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var list = await shiftTypes.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        return list.Select(CreateShiftTypeHandler.ToDto).ToList();
    }
}
