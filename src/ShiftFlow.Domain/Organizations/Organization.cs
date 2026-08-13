using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Organizations;

/// <summary>
/// Agregado raíz de organización (tenant lógico del MVP).
/// </summary>
public sealed class Organization
{
    /// <summary>
    /// Longitud máxima permitida para el nombre (INV-ORG-01).
    /// </summary>
    public const int NameMaxLength = 200;

    /// <summary>
    /// Valor por defecto de descanso mínimo (minutos). <c>0</c> desactiva HR-03.
    /// </summary>
    public const int DefaultMinimumRestMinutes = 0;

    private Organization()
    {
    }

    /// <summary>
    /// Identificador de la organización.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Nombre normalizado (trim) de la organización.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Indica si la organización está activa y puede usarse en altas hijas.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Descanso mínimo entre turnos Assigned del mismo empleado (minutos). Usado por HR-03.
    /// </summary>
    public int MinimumRestMinutes { get; private set; }

    #region Factory

    /// <summary>
    /// Crea una organización activa con nombre válido.
    /// </summary>
    /// <param name="name">Nombre obligatorio, no vacío tras trim.</param>
    /// <returns>Nueva organización con identificador generado.</returns>
    public static Organization Create(string name)
    {
        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = NormalizeName(name),
            IsActive = true,
            MinimumRestMinutes = DefaultMinimumRestMinutes
        };
    }

    #endregion

    #region Behavior

    /// <summary>
    /// Renombra la organización aplicando las mismas reglas de nombre que el alta.
    /// </summary>
    /// <param name="name">Nuevo nombre obligatorio.</param>
    public void Rename(string name) => Name = NormalizeName(name);

    /// <summary>
    /// Activa o desactiva la organización.
    /// </summary>
    /// <param name="isActive">Nuevo estado de activación.</param>
    public void SetActive(bool isActive) => IsActive = isActive;

    /// <summary>
    /// Configura el umbral de descanso mínimo entre turnos (HR-03).
    /// </summary>
    /// <param name="minutes">Minutos ≥ 0; <c>0</c> desactiva la regla.</param>
    public void SetMinimumRestMinutes(int minutes)
    {
        if (minutes < 0)
        {
            throw new DomainException(
                "INV-ORG-02",
                "El descanso mínimo entre turnos no puede ser negativo.");
        }

        MinimumRestMinutes = minutes;
    }

    #endregion

    #region Invariants

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("INV-ORG-01", "El nombre de la organización es obligatorio.");
        }

        string trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException(
                "INV-ORG-01",
                $"El nombre de la organización no puede superar {NameMaxLength} caracteres.");
        }

        return trimmed;
    }

    #endregion
}
