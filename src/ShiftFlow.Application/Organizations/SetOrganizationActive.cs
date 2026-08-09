using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

public sealed record SetOrganizationActiveCommand(Guid Id, bool IsActive) : IRequest<OrganizationDto>;

public sealed class SetOrganizationActiveHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<SetOrganizationActiveCommand, OrganizationDto>
{
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
