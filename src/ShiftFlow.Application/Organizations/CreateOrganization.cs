using MediatR;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

/// <summary>
/// Comando para dar de alta una organización.
/// </summary>
/// <param name="Name">Nombre de la nueva organización.</param>
public sealed record CreateOrganizationCommand(string Name) : IRequest<OrganizationDto>;

/// <summary>
/// DTO de lectura de una organización.
/// </summary>
/// <param name="Id">Identificador de la organización.</param>
/// <param name="Name">Nombre de la organización.</param>
/// <param name="IsActive">Indica si la organización está activa.</param>
public sealed record OrganizationDto(Guid Id, string Name, bool IsActive);

/// <summary>
/// Handler que crea una organización y la persiste.
/// </summary>
public sealed class CreateOrganizationHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrganizationCommand, OrganizationDto>
{
    /// <summary>
    /// Ejecuta el alta de la organización.
    /// </summary>
    /// <param name="request">Comando con el nombre a crear.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO de la organización creada.</returns>
    public async Task<OrganizationDto> Handle(
        CreateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = Organization.Create(request.Name);
        await organizations.AddAsync(organization, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(organization);
    }

    /// <summary>
    /// Mapea el agregado de organización a su DTO de aplicación.
    /// </summary>
    /// <param name="organization">Agregado de dominio.</param>
    /// <returns>DTO equivalente.</returns>
    internal static OrganizationDto ToDto(Organization organization) =>
        new(organization.Id, organization.Name, organization.IsActive);
}
