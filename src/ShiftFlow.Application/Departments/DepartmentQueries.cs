using MediatR;
using ShiftFlow.Domain.Departments;

namespace ShiftFlow.Application.Departments;

/// <summary>
/// Lista los departamentos de una organización.
/// </summary>
/// <param name="OrganizationId">Identificador de la organización.</param>
public sealed record ListDepartmentsByOrganizationQuery(Guid OrganizationId)
    : IRequest<IReadOnlyList<DepartmentDto>>;

/// <summary>
/// Handler de <see cref="ListDepartmentsByOrganizationQuery"/>.
/// </summary>
public sealed class ListDepartmentsByOrganizationHandler(IDepartmentRepository departments)
    : IRequestHandler<ListDepartmentsByOrganizationQuery, IReadOnlyList<DepartmentDto>>
{
    /// <summary>
    /// Devuelve los departamentos de la organización indicada.
    /// </summary>
    /// <param name="request">Consulta con el identificador de organización.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de DTOs de departamento.</returns>
    public async Task<IReadOnlyList<DepartmentDto>> Handle(
        ListDepartmentsByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var list = await departments.ListByOrganizationAsync(request.OrganizationId, cancellationToken);
        return list.Select(CreateDepartmentHandler.ToDto).ToList();
    }
}
