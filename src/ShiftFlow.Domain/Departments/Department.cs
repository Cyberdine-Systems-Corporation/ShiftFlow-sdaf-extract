using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Departments;

/// <summary>
/// Agregado de departamento perteneciente a una organización.
/// </summary>
public sealed class Department
{
    /// <summary>
    /// Longitud máxima permitida para el nombre (INV-DEP-02).
    /// </summary>
    public const int NameMaxLength = 200;

    private Department()
    {
    }

    /// <summary>
    /// Identificador del departamento.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Organización propietaria del departamento.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Nombre normalizado (trim) del departamento.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Indica si el departamento está activo.
    /// </summary>
    public bool IsActive { get; private set; }

    #region Factory

    /// <summary>
    /// Crea un departamento activo en una organización válida y activa.
    /// </summary>
    /// <param name="organizationId">Identificador de la organización (no vacío).</param>
    /// <param name="name">Nombre obligatorio del departamento.</param>
    /// <param name="organizationIsActive">Estado de la organización en el momento del alta (INV-DEP-01).</param>
    /// <returns>Nuevo departamento con identificador generado.</returns>
    public static Department Create(Guid organizationId, string name, bool organizationIsActive)
    {
        if (organizationId == Guid.Empty)
        {
            throw new DomainException("INV-DEP-01", "El departamento requiere una organización válida.");
        }

        if (!organizationIsActive)
        {
            throw new DomainException(
                "INV-DEP-01",
                "No se puede crear un departamento en una organización inactiva.");
        }

        return new Department
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Name = NormalizeName(name),
            IsActive = true
        };
    }

    #endregion

    #region Behavior

    /// <summary>
    /// Renombra el departamento aplicando las mismas reglas de nombre que el alta.
    /// </summary>
    /// <param name="name">Nuevo nombre obligatorio.</param>
    public void Rename(string name) => Name = NormalizeName(name);

    /// <summary>
    /// Activa o desactiva el departamento.
    /// </summary>
    /// <param name="isActive">Nuevo estado de activación.</param>
    public void SetActive(bool isActive) => IsActive = isActive;

    #endregion

    #region Invariants

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("INV-DEP-02", "El nombre del departamento es obligatorio.");
        }

        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException(
                "INV-DEP-02",
                $"El nombre del departamento no puede superar {NameMaxLength} caracteres.");
        }

        return trimmed;
    }

    #endregion
}
