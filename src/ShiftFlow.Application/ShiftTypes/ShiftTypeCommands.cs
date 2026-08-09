using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftTypes;

public sealed record UpdateShiftTypeCommand(
    Guid Id,
    string Name,
    string? Code,
    TimeOnly? DefaultStartTime,
    TimeOnly? DefaultEndTime) : IRequest<ShiftTypeDto>;

public sealed class UpdateShiftTypeHandler(
    IShiftTypeRepository shiftTypes,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateShiftTypeCommand, ShiftTypeDto>
{
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

public sealed record SetShiftTypeActiveCommand(Guid Id, bool IsActive) : IRequest<ShiftTypeDto>;

public sealed class SetShiftTypeActiveHandler(
    IShiftTypeRepository shiftTypes,
    IUnitOfWork unitOfWork) : IRequestHandler<SetShiftTypeActiveCommand, ShiftTypeDto>
{
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
