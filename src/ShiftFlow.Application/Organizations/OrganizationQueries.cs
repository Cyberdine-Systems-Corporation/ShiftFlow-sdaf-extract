using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

#region GetById

/// <summary>
/// Consulta una organización por identificador.
/// </summary>
/// <param name="Id">Identificador de la organización.</param>
public sealed record GetOrganizationByIdQuery(Guid Id) : IRequest<OrganizationDto>;

/// <summary>
/// Handler de <see cref="GetOrganizationByIdQuery"/>.
/// </summary>
public sealed class GetOrganizationByIdHandler(IOrganizationRepository organizations)
    : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
{
    /// <summary>
    /// Obtiene la organización o falla si no existe.
    /// </summary>
    /// <param name="request">Consulta con el identificador.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO de la organización.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    public async Task<OrganizationDto> Handle(
        GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var organization = await organizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.Id} no encontrada.");

        return CreateOrganizationHandler.ToDto(organization);
    }
}

#endregion

#region List

/// <summary>
/// Lista todas las organizaciones.
/// </summary>
public sealed record ListOrganizationsQuery : IRequest<IReadOnlyList<OrganizationDto>>;

/// <summary>
/// Handler de <see cref="ListOrganizationsQuery"/>.
/// </summary>
public sealed class ListOrganizationsHandler(IOrganizationRepository organizations)
    : IRequestHandler<ListOrganizationsQuery, IReadOnlyList<OrganizationDto>>
{
    /// <summary>
    /// Devuelve el listado de organizaciones.
    /// </summary>
    /// <param name="request">Consulta sin parámetros adicionales.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de DTOs de organización.</returns>
    public async Task<IReadOnlyList<OrganizationDto>> Handle(
        ListOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        var list = await organizations.ListAsync(cancellationToken);
        return list.Select(CreateOrganizationHandler.ToDto).ToList();
    }
}

#endregion
