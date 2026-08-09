using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;

namespace ShiftFlow.Application.Employees;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    Guid DepartmentId,
    string DisplayName,
    string? Email) : IRequest<EmployeeDto>;

public sealed class UpdateEmployeeHandler(
    IDepartmentRepository departments,
    IEmployeeRepository employees,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Empleado {request.Id} no encontrado.");

        var department = await departments.GetByIdAsync(request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException($"Departamento {request.DepartmentId} no encontrado.");

        await CreateEmployeeHandler.EnsureEmailUniqueAsync(
            employees,
            employee.OrganizationId,
            request.Email,
            employee.Id,
            cancellationToken);

        employee.Update(
            department.Id,
            department.OrganizationId,
            request.DisplayName,
            request.Email);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateEmployeeHandler.ToDto(employee);
    }
}

public sealed record SetEmployeeActiveCommand(Guid Id, bool IsActive) : IRequest<EmployeeDto>;

public sealed class SetEmployeeActiveHandler(
    IEmployeeRepository employees,
    IUnitOfWork unitOfWork) : IRequestHandler<SetEmployeeActiveCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(
        SetEmployeeActiveCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Empleado {request.Id} no encontrado.");

        employee.SetActive(request.IsActive);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateEmployeeHandler.ToDto(employee);
    }
}
