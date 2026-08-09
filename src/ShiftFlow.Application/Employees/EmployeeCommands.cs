using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;

namespace ShiftFlow.Application.Employees;

#region Update

/// <summary>
/// Comando para actualizar departamento, nombre y email de un empleado.
/// </summary>
/// <param name="Id">Identificador del empleado.</param>
/// <param name="DepartmentId">Nuevo departamento de asignación.</param>
/// <param name="DisplayName">Nuevo nombre visible.</param>
/// <param name="Email">Nuevo email opcional.</param>
public sealed record UpdateEmployeeCommand(
    Guid Id,
    Guid DepartmentId,
    string DisplayName,
    string? Email) : IRequest<EmployeeDto>;

/// <summary>
/// Handler que actualiza un empleado validando departamento y unicidad de email.
/// </summary>
public sealed class UpdateEmployeeHandler(
    IDepartmentRepository departments,
    IEmployeeRepository employees,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    /// <summary>
    /// Ejecuta la actualización del empleado.
    /// </summary>
    /// <param name="request">Comando con los nuevos datos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del empleado actualizado.</returns>
    /// <exception cref="NotFoundException">Si el empleado o el departamento no existen.</exception>
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

#endregion

#region SetActive

/// <summary>
/// Comando para activar o desactivar un empleado.
/// </summary>
/// <param name="Id">Identificador del empleado.</param>
/// <param name="IsActive">Nuevo estado de activación.</param>
public sealed record SetEmployeeActiveCommand(Guid Id, bool IsActive) : IRequest<EmployeeDto>;

/// <summary>
/// Handler que cambia el estado activo de un empleado.
/// </summary>
public sealed class SetEmployeeActiveHandler(
    IEmployeeRepository employees,
    IUnitOfWork unitOfWork) : IRequestHandler<SetEmployeeActiveCommand, EmployeeDto>
{
    /// <summary>
    /// Ejecuta el cambio de activación del empleado.
    /// </summary>
    /// <param name="request">Comando con identificador y estado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del empleado actualizado.</returns>
    /// <exception cref="NotFoundException">Si el empleado no existe.</exception>
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

#endregion
