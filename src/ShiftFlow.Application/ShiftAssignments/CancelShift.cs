using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.ShiftAssignments;

namespace ShiftFlow.Application.ShiftAssignments;

/// <summary>
/// Comando para cancelar una asignación <c>Assigned</c>.
/// </summary>
/// <param name="ShiftAssignmentId">Identificador de la asignación.</param>
public sealed record CancelShiftCommand(Guid ShiftAssignmentId) : IRequest<ShiftAssignmentDto>;

/// <summary>
/// Handler que cancela una asignación vigente (INV-ASN-06).
/// </summary>
public sealed class CancelShiftHandler(
    IShiftAssignmentRepository assignments,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelShiftCommand, ShiftAssignmentDto>
{
    /// <summary>
    /// Ejecuta la cancelación.
    /// </summary>
    /// <param name="request">Comando de cancelación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO de la asignación cancelada.</returns>
    /// <exception cref="NotFoundException">Si la asignación no existe.</exception>
    /// <exception cref="DomainException">Si no está en estado Assigned.</exception>
    public async Task<ShiftAssignmentDto> Handle(
        CancelShiftCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await assignments.GetByIdAsync(request.ShiftAssignmentId, cancellationToken)
            ?? throw new NotFoundException($"Asignación {request.ShiftAssignmentId} no encontrada.");

        assignment.Cancel();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return AssignShiftHandler.ToDto(assignment);
    }
}
