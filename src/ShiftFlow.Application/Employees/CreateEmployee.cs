using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Employees;

public sealed record CreateEmployeeCommand(
    Guid OrganizationId,
    Guid DepartmentId,
    string DisplayName,
    string? Email) : IRequest<EmployeeDto>;

public sealed record EmployeeDto(
    Guid Id,
    Guid OrganizationId,
    Guid DepartmentId,
    string DisplayName,
    string? Email,
    bool IsActive);

public sealed class CreateEmployeeHandler(
    IOrganizationRepository organizations,
    IDepartmentRepository departments,
    IEmployeeRepository employees,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        _ = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        var department = await departments.GetByIdAsync(request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException($"Departamento {request.DepartmentId} no encontrado.");

        await EnsureEmailUniqueAsync(request.OrganizationId, request.Email, null, cancellationToken);

        var employee = Employee.Create(
            request.OrganizationId,
            department.Id,
            department.OrganizationId,
            department.IsActive,
            request.DisplayName,
            request.Email);

        await employees.AddAsync(employee, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(employee);
    }

    internal static EmployeeDto ToDto(Employee employee) =>
        new(
            employee.Id,
            employee.OrganizationId,
            employee.DepartmentId,
            employee.DisplayName,
            employee.Email,
            employee.IsActive);

    internal static async Task EnsureEmailUniqueAsync(
        IEmployeeRepository employees,
        Guid organizationId,
        string? email,
        Guid? excludingEmployeeId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        if (await employees.ExistsWithEmailAsync(organizationId, email.Trim(), excludingEmployeeId, cancellationToken))
        {
            throw new DomainException(
                "INV-EMP-01",
                "Ya existe un empleado con ese email en la organización.");
        }
    }

    private Task EnsureEmailUniqueAsync(
        Guid organizationId,
        string? email,
        Guid? excludingEmployeeId,
        CancellationToken cancellationToken) =>
        EnsureEmailUniqueAsync(employees, organizationId, email, excludingEmployeeId, cancellationToken);
}
