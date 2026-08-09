using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

/// <summary>
/// Comando para activar o desactivar una organización.
/// </summary>
/// <param name="Id">Identificador de la organización.</param>
/// <param name="IsActive">Nuevo estado de activación.</param>
public sealed record SetOrganizationActiveCommand(Guid Id, bool IsActive) : IRequest<OrganizationDto>;

/// <summary>
/// Handler que cambia el estado activo de una organización.
/// </summary>
public sealed class SetOrganizationActiveHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<SetOrganizationActiveCommand, OrganizationDto>
{
    /// <summary>
    /// Ejecuta el cambio de activación de la organización.
    /// </summary>
    /// <param name="request">Comando con identificador y estado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO de la organización actualizada.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    public async Task<OrganizationDto> Handle(
        SetOrganizationActiveCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.Id} no encontrada.");

        organization.SetActive(request.IsActive);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateOrganizationHandler.ToDto(organization);
    }
}
