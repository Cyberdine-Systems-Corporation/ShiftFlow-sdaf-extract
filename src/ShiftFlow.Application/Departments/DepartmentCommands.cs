using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;

namespace ShiftFlow.Application.Departments;

public sealed record RenameDepartmentCommand(Guid Id, string Name) : IRequest<DepartmentDto>;

public sealed class RenameDepartmentHandler(
    IDepartmentRepository departments,
    IUnitOfWork unitOfWork) : IRequestHandler<RenameDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(
        RenameDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await departments.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Departamento {request.Id} no encontrado.");

        if (await departments.ExistsWithNameAsync(
                department.OrganizationId,
                request.Name,
                department.Id,
                cancellationToken))
        {
            throw new DomainException(
                "INV-DEP-02",
                "Ya existe un departamento con ese nombre en la organización.");
        }

        department.Rename(request.Name);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateDepartmentHandler.ToDto(department);
    }
}

public sealed record SetDepartmentActiveCommand(Guid Id, bool IsActive) : IRequest<DepartmentDto>;

public sealed class SetDepartmentActiveHandler(
    IDepartmentRepository departments,
    IUnitOfWork unitOfWork) : IRequestHandler<SetDepartmentActiveCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(
        SetDepartmentActiveCommand request,
        CancellationToken cancellationToken)
    {
        var department = await departments.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Departamento {request.Id} no encontrado.");

        department.SetActive(request.IsActive);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateDepartmentHandler.ToDto(department);
    }
}
