using MediatR;
using ShiftFlow.Domain.Departments;

namespace ShiftFlow.Application.Departments;

public sealed record ListDepartmentsByOrganizationQuery(Guid OrganizationId)
    : IRequest<IReadOnlyList<DepartmentDto>>;

public sealed class ListDepartmentsByOrganizationHandler(IDepartmentRepository departments)
    : IRequestHandler<ListDepartmentsByOrganizationQuery, IReadOnlyList<DepartmentDto>>
{
    public async Task<IReadOnlyList<DepartmentDto>> Handle(
        ListDepartmentsByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var list = await departments.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        return list.Select(CreateDepartmentHandler.ToDto).ToList();
    }
}
