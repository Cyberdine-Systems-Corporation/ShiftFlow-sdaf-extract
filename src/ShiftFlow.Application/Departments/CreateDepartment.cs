using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Departments;

public sealed record CreateDepartmentCommand(Guid OrganizationId, string Name) : IRequest<DepartmentDto>;

public sealed record DepartmentDto(Guid Id, Guid OrganizationId, string Name, bool IsActive);

public sealed class CreateDepartmentHandler(
    IOrganizationRepository organizations,
    IDepartmentRepository departments,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        if (await departments.ExistsWithNameAsync(request.OrganizationId, request.Name, null, cancellationToken))
        {
            throw new DomainException(
                "INV-DEP-02",
                "Ya existe un departamento con ese nombre en la organización.");
        }

        var department = Department.Create(organization.Id, request.Name, organization.IsActive);
        await departments.AddAsync(department, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(department);
    }

    internal static DepartmentDto ToDto(Department department) =>
        new(department.Id, department.OrganizationId, department.Name, department.IsActive);
}
