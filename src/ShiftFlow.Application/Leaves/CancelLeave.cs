using MediatR;
using ShiftFlow.Application.Common;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Leaves;

namespace ShiftFlow.Application.Leaves;

/// <summary>
/// Comando para cancelar un leave <c>Active</c>.
/// </summary>
/// <param name="LeaveId">Identificador del leave.</param>
public sealed record CancelLeaveCommand(Guid LeaveId) : IRequest<LeaveDto>;

/// <summary>
/// Handler que cancela un leave vigente (INV-LEA-05).
/// </summary>
public sealed class CancelLeaveHandler(
    ILeaveRepository leaves,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelLeaveCommand, LeaveDto>
{
    /// <summary>
    /// Ejecuta la cancelación.
    /// </summary>
    /// <param name="request">Comando.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del leave cancelado.</returns>
    /// <exception cref="NotFoundException">Si el leave no existe.</exception>
    /// <exception cref="DomainException">Si no está en estado Active.</exception>
    public async Task<LeaveDto> Handle(CancelLeaveCommand request, CancellationToken cancellationToken)
    {
        Leave? leave = await leaves.GetByIdAsync(request.LeaveId, cancellationToken)
            ?? throw new NotFoundException($"Ausencia {request.LeaveId} no encontrada.");

        leave.Cancel();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return RegisterLeaveHandler.ToDto(leave);
    }
}
