using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftTypes;

/// <summary>
/// Comando para dar de alta un tipo de turno en una organización.
/// </summary>
/// <param name="OrganizationId">Organización propietaria del tipo de turno.</param>
/// <param name="Name">Nombre del tipo de turno.</param>
/// <param name="Code">Código opcional único en la organización.</param>
/// <param name="DefaultStartTime">Hora de inicio por defecto opcional.</param>
/// <param name="DefaultEndTime">Hora de fin por defecto opcional.</param>
public sealed record CreateShiftTypeCommand(
    Guid OrganizationId,
    string Name,
    string? Code,
    TimeOnly? DefaultStartTime,
    TimeOnly? DefaultEndTime) : IRequest<ShiftTypeDto>;

/// <summary>
/// DTO de lectura de un tipo de turno.
/// </summary>
/// <param name="Id">Identificador del tipo de turno.</param>
/// <param name="OrganizationId">Organización a la que pertenece.</param>
/// <param name="Name">Nombre del tipo de turno.</param>
/// <param name="Code">Código opcional.</param>
/// <param name="DefaultStartTime">Hora de inicio por defecto opcional.</param>
/// <param name="DefaultEndTime">Hora de fin por defecto opcional.</param>
/// <param name="IsActive">Indica si el tipo de turno está activo.</param>
public sealed record ShiftTypeDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string? Code,
    TimeOnly? DefaultStartTime,
    TimeOnly? DefaultEndTime,
    bool IsActive);

/// <summary>
/// Handler que crea un tipo de turno validando organización y unicidad de nombre/código.
/// </summary>
public sealed class CreateShiftTypeHandler(
    IOrganizationRepository organizations,
    IShiftTypeRepository shiftTypes,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateShiftTypeCommand, ShiftTypeDto>
{
    /// <summary>
    /// Ejecuta el alta del tipo de turno.
    /// </summary>
    /// <param name="request">Comando con datos del tipo de turno.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del tipo de turno creado.</returns>
    /// <exception cref="NotFoundException">Si la organización no existe.</exception>
    public async Task<ShiftTypeDto> Handle(
        CreateShiftTypeCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizations.GetByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new NotFoundException($"Organización {request.OrganizationId} no encontrada.");

        await EnsureUniquenessAsync(
            shiftTypes,
            request.OrganizationId,
            request.Name,
            request.Code,
            excludingShiftTypeId: null,
            cancellationToken);

        var shiftType = ShiftType.Create(
            organization.Id,
            organization.IsActive,
            request.Name,
            request.Code,
            request.DefaultStartTime,
            request.DefaultEndTime);

        await shiftTypes.AddAsync(shiftType, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(shiftType);
    }

    /// <summary>
    /// Mapea el agregado de tipo de turno a su DTO de aplicación.
    /// </summary>
    /// <param name="shiftType">Agregado de dominio.</param>
    /// <returns>DTO equivalente.</returns>
    internal static ShiftTypeDto ToDto(ShiftType shiftType) =>
        new(
            shiftType.Id,
            shiftType.OrganizationId,
            shiftType.Name,
            shiftType.Code,
            shiftType.DefaultStartTime,
            shiftType.DefaultEndTime,
            shiftType.IsActive);

    /// <summary>
    /// Garantiza unicidad de nombre y, si aplica, de código dentro de la organización.
    /// </summary>
    /// <param name="shiftTypes">Repositorio de tipos de turno.</param>
    /// <param name="organizationId">Organización donde se comprueba la unicidad.</param>
    /// <param name="name">Nombre candidato.</param>
    /// <param name="code">Código candidato; se ignora si es nulo o vacío.</param>
    /// <param name="excludingShiftTypeId">Tipo a excluir (p. ej. en actualización).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    internal static async Task EnsureUniquenessAsync(
        IShiftTypeRepository shiftTypes,
        Guid organizationId,
        string name,
        string? code,
        Guid? excludingShiftTypeId,
        CancellationToken cancellationToken)
    {
        if (await shiftTypes.ExistsWithNameAsync(organizationId, name, excludingShiftTypeId, cancellationToken))
        {
            throw new DomainException(
                "INV-STT-02",
                "Ya existe un tipo de turno con ese nombre en la organización.");
        }

        if (!string.IsNullOrWhiteSpace(code) &&
            await shiftTypes.ExistsWithCodeAsync(organizationId, code, excludingShiftTypeId, cancellationToken))
        {
            throw new DomainException(
                "INV-STT-03",
                "Ya existe un tipo de turno con ese código en la organización.");
        }
    }
}
