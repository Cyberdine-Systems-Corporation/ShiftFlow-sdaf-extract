namespace ShiftFlow.Domain.Organizations;

/// <summary>
/// Puerto de persistencia de organizaciones.
/// </summary>
public interface IOrganizationRepository
{
    /// <summary>
    /// Obtiene una organización por identificador.
    /// </summary>
    /// <param name="id">Identificador de la organización.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>La organización o <c>null</c> si no existe.</returns>
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista todas las organizaciones.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de organizaciones.</returns>
    Task<IReadOnlyList<Organization>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade una organización al almacén.
    /// </summary>
    /// <param name="organization">Agregado a persistir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(Organization organization, CancellationToken cancellationToken = default);
}
