using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Application.ShiftTypes;

public sealed record CreateShiftTypeCommand(
    Guid OrganizationId,
    string Name,
    string? Code,
    TimeOnly? DefaultStartTime,
    TimeOnly? DefaultEndTime) : IRequest<ShiftTypeDto>;

public sealed record ShiftTypeDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string? Code,
    TimeOnly? DefaultStartTime,
    TimeOnly? DefaultEndTime,
    bool IsActive);

public sealed class CreateShiftTypeHandler(
    IOrganizationRepository organizations,
    IShiftTypeRepository shiftTypes,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateShiftTypeCommand, ShiftTypeDto>
{
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

    internal static ShiftTypeDto ToDto(ShiftType shiftType) =>
        new(
            shiftType.Id,
            shiftType.OrganizationId,
            shiftType.Name,
            shiftType.Code,
            shiftType.DefaultStartTime,
            shiftType.DefaultEndTime,
            shiftType.IsActive);

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
