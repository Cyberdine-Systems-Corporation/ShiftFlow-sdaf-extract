using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Employees;

/// <summary>
/// Comando para dar de alta un empleado en un departamento.
/// </summary>
/// <param name="OrganizationId">Organización del empleado.</param>
/// <param name="DepartmentId">Departamento de asignación.</param>
/// <param name="DisplayName">Nombre visible del empleado.</param>
/// <param name="Email">Email opcional; si se informa debe ser único en la organización.</param>
public sealed record CreateEmployeeCommand(
    Guid OrganizationId,
    Guid DepartmentId,
    string DisplayName,
    string? Email) : IRequest<EmployeeDto>;

/// <summary>
/// DTO de lectura de un empleado.
/// </summary>
/// <param name="Id">Identificador del empleado.</param>
/// <param name="OrganizationId">Organización a la que pertenece.</param>
/// <param name="DepartmentId">Departamento de asignación.</param>
/// <param name="DisplayName">Nombre visible.</param>
/// <param name="Email">Email opcional.</param>
/// <param name="IsActive">Indica si el empleado está activo.</param>
public sealed record EmployeeDto(
    Guid Id,
    Guid OrganizationId,
    Guid DepartmentId,
    string DisplayName,
    string? Email,
    bool IsActive);

/// <summary>
/// Handler que crea un empleado validando organización, departamento y unicidad de email.
/// </summary>
public sealed class CreateEmployeeHandler(
    IOrganizationRepository organizations,
    IDepartmentRepository departments,
    IEmployeeRepository employees,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    /// <summary>
    /// Ejecuta el alta del empleado.
    /// </summary>
    /// <param name="request">Comando con datos del empleado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del empleado creado.</returns>
    /// <exception cref="NotFoundException">Si la organización o el departamento no existen.</exception>
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

    /// <summary>
    /// Mapea el agregado de empleado a su DTO de aplicación.
    /// </summary>
    /// <param name="employee">Agregado de dominio.</param>
    /// <returns>DTO equivalente.</returns>
    internal static EmployeeDto ToDto(Employee employee) =>
        new(
            employee.Id,
            employee.OrganizationId,
            employee.DepartmentId,
            employee.DisplayName,
            employee.Email,
            employee.IsActive);

    /// <summary>
    /// Garantiza que el email, si viene informado, no esté ya usado en la organización.
    /// </summary>
    /// <param name="employees">Repositorio de empleados.</param>
    /// <param name="organizationId">Organización donde se comprueba la unicidad.</param>
    /// <param name="email">Email candidato; se ignora si es nulo o vacío.</param>
    /// <param name="excludingEmployeeId">Empleado a excluir (p. ej. en actualización).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
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
