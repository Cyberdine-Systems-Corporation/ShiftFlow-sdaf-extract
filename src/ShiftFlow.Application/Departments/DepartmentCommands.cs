using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;

namespace ShiftFlow.Application.Departments;

#region Rename

/// <summary>
/// Comando para renombrar un departamento.
/// </summary>
/// <param name="Id">Identificador del departamento.</param>
/// <param name="Name">Nuevo nombre.</param>
public sealed record RenameDepartmentCommand(Guid Id, string Name) : IRequest<DepartmentDto>;

/// <summary>
/// Handler que renombra un departamento validando unicidad de nombre.
/// </summary>
public sealed class RenameDepartmentHandler(
    IDepartmentRepository departments,
    IUnitOfWork unitOfWork) : IRequestHandler<RenameDepartmentCommand, DepartmentDto>
{
    /// <summary>
    /// Ejecuta el renombrado del departamento.
    /// </summary>
    /// <param name="request">Comando con identificador y nuevo nombre.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del departamento actualizado.</returns>
    /// <exception cref="NotFoundException">Si el departamento no existe.</exception>
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

#endregion

#region SetActive

/// <summary>
/// Comando para activar o desactivar un departamento.
/// </summary>
/// <param name="Id">Identificador del departamento.</param>
/// <param name="IsActive">Nuevo estado de activación.</param>
public sealed record SetDepartmentActiveCommand(Guid Id, bool IsActive) : IRequest<DepartmentDto>;

/// <summary>
/// Handler que cambia el estado activo de un departamento.
/// </summary>
public sealed class SetDepartmentActiveHandler(
    IDepartmentRepository departments,
    IUnitOfWork unitOfWork) : IRequestHandler<SetDepartmentActiveCommand, DepartmentDto>
{
    /// <summary>
    /// Ejecuta el cambio de activación del departamento.
    /// </summary>
    /// <param name="request">Comando con identificador y estado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del departamento actualizado.</returns>
    /// <exception cref="NotFoundException">Si el departamento no existe.</exception>
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

#endregion
