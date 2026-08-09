using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Organizations;

namespace ShiftFlow.Application.Departments;

/// <summary>
/// Comando para dar de alta un departamento en una organización.
/// </summary>
/// <param name="OrganizationId">Organización propietaria del departamento.</param>
/// <param name="Name">Nombre del nuevo departamento.</param>
public sealed record CreateDepartmentCommand(Guid OrganizationId, string Name) : IRequest<DepartmentDto>;

/// <summary>
/// DTO de lectura de un departamento.
/// </summary>
/// <param name="Id">Identificador del departamento.</param>
/// <param name="OrganizationId">Organización a la que pertenece.</param>
/// <param name="Name">Nombre del departamento.</param>
/// <param name="IsActive">Indica si el departamento está activo.</param>
public sealed record DepartmentDto(Guid Id, Guid OrganizationId, string Name, bool IsActive);

/// <summary>
/// Handler que crea un departamento validando organización y unicidad de nombre.
/// </summary>
public sealed class CreateDepartmentHandler(
    IOrganizationRepository organizations,
    IDepartmentRepository departments,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    /// <summary>
    /// Ejecuta el alta del departamento.
    /// </summary>
    /// <param name="request">Comando con organización y nombre.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del departamento creado.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
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

    /// <summary>
    /// Mapea el agregado de departamento a su DTO de aplicación.
    /// </summary>
    /// <param name="department">Agregado de dominio.</param>
    /// <returns>DTO equivalente.</returns>
    internal static DepartmentDto ToDto(Department department) =>
        new(department.Id, department.OrganizationId, department.Name, department.IsActive);
}
