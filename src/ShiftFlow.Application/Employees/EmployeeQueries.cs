using MediatR;
using ShiftFlow.Domain.Employees;

namespace ShiftFlow.Application.Employees;

#region ByOrganization

/// <summary>
/// Lista los empleados de una organización.
/// </summary>
/// <param name="OrganizationId">Identificador de la organización.</param>
public sealed record ListEmployeesByOrganizationQuery(Guid OrganizationId)
    : IRequest<IReadOnlyList<EmployeeDto>>;

/// <summary>
/// Handler de <see cref="ListEmployeesByOrganizationQuery"/>.
/// </summary>
public sealed class ListEmployeesByOrganizationHandler(IEmployeeRepository employees)
    : IRequestHandler<ListEmployeesByOrganizationQuery, IReadOnlyList<EmployeeDto>>
{
    /// <summary>
    /// Devuelve los empleados de la organización indicada.
    /// </summary>
    /// <param name="request">Consulta con el identificador de organización.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de DTOs de empleado.</returns>
    public async Task<IReadOnlyList<EmployeeDto>> Handle(
        ListEmployeesByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var list = await employees.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        return list.Select(CreateEmployeeHandler.ToDto).ToList();
    }
}

#endregion

#region ByDepartment

/// <summary>
/// Lista los empleados de un departamento.
/// </summary>
/// <param name="DepartmentId">Identificador del departamento.</param>
public sealed record ListEmployeesByDepartmentQuery(Guid DepartmentId)
    : IRequest<IReadOnlyList<EmployeeDto>>;

/// <summary>
/// Handler de <see cref="ListEmployeesByDepartmentQuery"/>.
/// </summary>
public sealed class ListEmployeesByDepartmentHandler(IEmployeeRepository employees)
    : IRequestHandler<ListEmployeesByDepartmentQuery, IReadOnlyList<EmployeeDto>>
{
    /// <summary>
    /// Devuelve los empleados del departamento indicado.
    /// </summary>
    /// <param name="request">Consulta con el identificador de departamento.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de DTOs de empleado.</returns>
    public async Task<IReadOnlyList<EmployeeDto>> Handle(
        ListEmployeesByDepartmentQuery request,
        CancellationToken cancellationToken)
    {
        var list = await employees.ListByDepartmentAsync(request.DepartmentId, cancellationToken);
        return list.Select(CreateEmployeeHandler.ToDto).ToList();
    }
}

#endregion
