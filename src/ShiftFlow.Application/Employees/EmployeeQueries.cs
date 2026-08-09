using MediatR;
using ShiftFlow.Domain.Employees;

namespace ShiftFlow.Application.Employees;

public sealed record ListEmployeesByOrganizationQuery(Guid OrganizationId)
    : IRequest<IReadOnlyList<EmployeeDto>>;

public sealed class ListEmployeesByOrganizationHandler(IEmployeeRepository employees)
    : IRequestHandler<ListEmployeesByOrganizationQuery, IReadOnlyList<EmployeeDto>>
{
    public async Task<IReadOnlyList<EmployeeDto>> Handle(
        ListEmployeesByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var list = await employees.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        return list.Select(CreateEmployeeHandler.ToDto).ToList();
    }
}

public sealed record ListEmployeesByDepartmentQuery(Guid DepartmentId)
    : IRequest<IReadOnlyList<EmployeeDto>>;

public sealed class ListEmployeesByDepartmentHandler(IEmployeeRepository employees)
    : IRequestHandler<ListEmployeesByDepartmentQuery, IReadOnlyList<EmployeeDto>>
{
    public async Task<IReadOnlyList<EmployeeDto>> Handle(
        ListEmployeesByDepartmentQuery request,
        CancellationToken cancellationToken)
    {
        var list = await employees.ListByDepartmentAsync(request.DepartmentId, cancellationToken);
        return list.Select(CreateEmployeeHandler.ToDto).ToList();
    }
}
