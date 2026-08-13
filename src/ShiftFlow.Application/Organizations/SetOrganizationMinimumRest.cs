using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

/// <summary>
/// Comando para configurar el descanso mínimo entre turnos (HR-03).
/// </summary>
/// <param name="Id">Organización.</param>
/// <param name="MinimumRestMinutes">Minutos ≥ 0; 0 desactiva HR-03.</param>
public sealed record SetOrganizationMinimumRestCommand(Guid Id, int MinimumRestMinutes)
    : IRequest<OrganizationDto>;

/// <summary>
/// Handler que actualiza el umbral de descanso mínimo.
/// </summary>
public sealed class SetOrganizationMinimumRestHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<SetOrganizationMinimumRestCommand, OrganizationDto>
{
    /// <inheritdoc />
    public async Task<OrganizationDto> Handle(
        SetOrganizationMinimumRestCommand request,
        CancellationToken cancellationToken)
    {
        Organization organization = await organizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.Id} no encontrada.");

        organization.SetMinimumRestMinutes(request.MinimumRestMinutes);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateOrganizationHandler.ToDto(organization);
    }
}
