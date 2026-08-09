using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Organizations;

public sealed record GetOrganizationByIdQuery(Guid Id) : IRequest<OrganizationDto>;

public sealed class GetOrganizationByIdHandler(IOrganizationRepository organizations)
    : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
{
    public async Task<OrganizationDto> Handle(
        GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var organization = await organizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.Id} no encontrada.");

        return CreateOrganizationHandler.ToDto(organization);
    }
}

public sealed record ListOrganizationsQuery : IRequest<IReadOnlyList<OrganizationDto>>;

public sealed class ListOrganizationsHandler(IOrganizationRepository organizations)
    : IRequestHandler<ListOrganizationsQuery, IReadOnlyList<OrganizationDto>>
{
    public async Task<IReadOnlyList<OrganizationDto>> Handle(
        ListOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        var list = await organizations.ListAsync(cancellationToken);
        return list.Select(CreateOrganizationHandler.ToDto).ToList();
    }
}
