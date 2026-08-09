using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

/// <summary>
/// Comando para renombrar una organización existente.
/// </summary>
/// <param name="Id">Identificador de la organización.</param>
/// <param name="Name">Nuevo nombre.</param>
public sealed record RenameOrganizationCommand(Guid Id, string Name) : IRequest<OrganizationDto>;

/// <summary>
/// Handler que renombra una organización y persiste el cambio.
/// </summary>
public sealed class RenameOrganizationHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<RenameOrganizationCommand, OrganizationDto>
{
    /// <summary>
    /// Ejecuta el renombrado de la organización.
    /// </summary>
    /// <param name="request">Comando con identificador y nuevo nombre.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO de la organización actualizada.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    public async Task<OrganizationDto> Handle(
        RenameOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.Id} no encontrada.");

        organization.Rename(request.Name);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateOrganizationHandler.ToDto(organization);
    }
}
