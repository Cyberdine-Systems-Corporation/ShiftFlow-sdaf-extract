namespace ShiftFlow.Domain.Common;

/// <summary>
/// Unidad de trabajo para persistir cambios del dominio en una transacción.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste los cambios pendientes del contexto.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Número de entidades afectadas.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
