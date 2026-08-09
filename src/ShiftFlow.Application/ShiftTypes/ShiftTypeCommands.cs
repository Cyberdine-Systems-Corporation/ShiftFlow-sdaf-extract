using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftTypes;

#region Update

/// <summary>
/// Comando para actualizar nombre, código y horarios por defecto de un tipo de turno.
/// </summary>
/// <param name="Id">Identificador del tipo de turno.</param>
/// <param name="Name">Nuevo nombre.</param>
/// <param name="Code">Nuevo código opcional.</param>
/// <param name="DefaultStartTime">Nueva hora de inicio por defecto opcional.</param>
/// <param name="DefaultEndTime">Nueva hora de fin por defecto opcional.</param>
public sealed record UpdateShiftTypeCommand(
    Guid Id,
    string Name,
    string? Code,
    TimeOnly? DefaultStartTime,
    TimeOnly? DefaultEndTime) : IRequest<ShiftTypeDto>;

/// <summary>
/// Handler que actualiza un tipo de turno validando unicidad de nombre/código.
/// </summary>
public sealed class UpdateShiftTypeHandler(
    IShiftTypeRepository shiftTypes,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateShiftTypeCommand, ShiftTypeDto>
{
    /// <summary>
    /// Ejecuta la actualización del tipo de turno.
    /// </summary>
    /// <param name="request">Comando con los nuevos datos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del tipo de turno actualizado.</returns>
    /// <exception cref="NotFoundException">Si el tipo de turno no existe.</exception>
    public async Task<ShiftTypeDto> Handle(
        UpdateShiftTypeCommand request,
        CancellationToken cancellationToken)
    {
        var shiftType = await shiftTypes.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Tipo de turno {request.Id} no encontrado.");

        await CreateShiftTypeHandler.EnsureUniquenessAsync(
            shiftTypes,
            shiftType.OrganizationId,
            request.Name,
            request.Code,
            shiftType.Id,
            cancellationToken);

        shiftType.Update(request.Name, request.Code, request.DefaultStartTime, request.DefaultEndTime);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateShiftTypeHandler.ToDto(shiftType);
    }
}

#endregion

#region SetActive

/// <summary>
/// Comando para activar o desactivar un tipo de turno.
/// </summary>
/// <param name="Id">Identificador del tipo de turno.</param>
/// <param name="IsActive">Nuevo estado de activación.</param>
public sealed record SetShiftTypeActiveCommand(Guid Id, bool IsActive) : IRequest<ShiftTypeDto>;

/// <summary>
/// Handler que cambia el estado activo de un tipo de turno.
/// </summary>
public sealed class SetShiftTypeActiveHandler(
    IShiftTypeRepository shiftTypes,
    IUnitOfWork unitOfWork) : IRequestHandler<SetShiftTypeActiveCommand, ShiftTypeDto>
{
    /// <summary>
    /// Ejecuta el cambio de activación del tipo de turno.
    /// </summary>
    /// <param name="request">Comando con identificador y estado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del tipo de turno actualizado.</returns>
    /// <exception cref="NotFoundException">Si el tipo de turno no existe.</exception>
    public async Task<ShiftTypeDto> Handle(
        SetShiftTypeActiveCommand request,
        CancellationToken cancellationToken)
    {
        var shiftType = await shiftTypes.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Tipo de turno {request.Id} no encontrado.");

        shiftType.SetActive(request.IsActive);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateShiftTypeHandler.ToDto(shiftType);
    }
}

#endregion
