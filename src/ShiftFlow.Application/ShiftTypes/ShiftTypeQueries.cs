using MediatR;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftTypes;

public sealed record ListShiftTypesByOrganizationQuery(Guid OrganizationId)
    : IRequest<IReadOnlyList<ShiftTypeDto>>;

public sealed class ListShiftTypesByOrganizationHandler(IShiftTypeRepository shiftTypes)
    : IRequestHandler<ListShiftTypesByOrganizationQuery, IReadOnlyList<ShiftTypeDto>>
{
    public async Task<IReadOnlyList<ShiftTypeDto>> Handle(
        ListShiftTypesByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var list = await shiftTypes.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        return list.Select(CreateShiftTypeHandler.ToDto).ToList();
    }
}
