namespace ShiftFlow.Domain.ShiftTypes;

/// <summary>
/// Puerto de persistencia de tipos de turno.
/// </summary>
public interface IShiftTypeRepository
{
    /// <summary>
    /// Obtiene un tipo de turno por identificador.
    /// </summary>
    /// <param name="id">Identificador del tipo de turno.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El tipo de turno o <c>null</c> si no existe.</returns>
    Task<ShiftType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista los tipos de turno de una organización.
    /// </summary>
    /// <param name="organizationId">Organización propietaria.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de tipos de turno de la organización.</returns>
    Task<IReadOnlyList<ShiftType>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Indica si ya existe un tipo de turno con el mismo nombre en la organización.
    /// </summary>
    /// <param name="organizationId">Organización en la que buscar.</param>
    /// <param name="name">Nombre a comprobar.</param>
    /// <param name="excludingShiftTypeId">Tipo de turno a excluir (actualizaciones).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>true</c> si el nombre ya está en uso.</returns>
    Task<bool> ExistsWithNameAsync(
        Guid organizationId,
        string name,
        Guid? excludingShiftTypeId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Indica si ya existe un tipo de turno con el mismo código en la organización.
    /// </summary>
    /// <param name="organizationId">Organización en la que buscar.</param>
    /// <param name="code">Código a comprobar.</param>
    /// <param name="excludingShiftTypeId">Tipo de turno a excluir (actualizaciones).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>true</c> si el código ya está en uso.</returns>
    Task<bool> ExistsWithCodeAsync(
        Guid organizationId,
        string code,
        Guid? excludingShiftTypeId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un tipo de turno al almacén.
    /// </summary>
    /// <param name="shiftType">Agregado a persistir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(ShiftType shiftType, CancellationToken cancellationToken = default);
}
