using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

public sealed record RenameOrganizationCommand(Guid Id, string Name) : IRequest<OrganizationDto>;

public sealed class RenameOrganizationHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<RenameOrganizationCommand, OrganizationDto>
{
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
