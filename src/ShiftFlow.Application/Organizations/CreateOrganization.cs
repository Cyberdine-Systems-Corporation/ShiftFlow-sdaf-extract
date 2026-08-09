using MediatR;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

public sealed record CreateOrganizationCommand(string Name) : IRequest<OrganizationDto>;

public sealed record OrganizationDto(Guid Id, string Name, bool IsActive);

public sealed class CreateOrganizationHandler(
    IOrganizationRepository organizations,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrganizationCommand, OrganizationDto>
{
    public async Task<OrganizationDto> Handle(
        CreateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = Organization.Create(request.Name);
        await organizations.AddAsync(organization, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(organization);
    }

    internal static OrganizationDto ToDto(Organization organization) =>
        new(organization.Id, organization.Name, organization.IsActive);
}
