using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.ShiftTypes;

/// <summary>
/// Agregado de tipo de turno (plantilla horaria por organización).
/// </summary>
public sealed class ShiftType
{
    /// <summary>
    /// Longitud máxima del nombre (INV-STT-02).
    /// </summary>
    public const int NameMaxLength = 200;

    /// <summary>
    /// Longitud máxima del código opcional (INV-STT-03).
    /// </summary>
    public const int CodeMaxLength = 50;

    private ShiftType()
    {
    }

    /// <summary>
    /// Identificador del tipo de turno.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Organización propietaria del tipo de turno.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Nombre normalizado (trim) del tipo de turno.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Código opcional normalizado; <c>null</c> si no se informa.
    /// </summary>
    public string? Code { get; private set; }

    /// <summary>
    /// Hora de inicio por defecto; opcional junto con <see cref="DefaultEndTime"/>.
    /// </summary>
    public TimeOnly? DefaultStartTime { get; private set; }

    /// <summary>
    /// Hora de fin por defecto; opcional junto con <see cref="DefaultStartTime"/>.
    /// </summary>
    public TimeOnly? DefaultEndTime { get; private set; }

    /// <summary>
    /// Indica si el tipo de turno está activo.
    /// </summary>
    public bool IsActive { get; private set; }

    #region Factory

    /// <summary>
    /// Crea un tipo de turno activo en una organización válida y activa.
    /// </summary>
    /// <param name="organizationId">Identificador de la organización (no vacío).</param>
    /// <param name="organizationIsActive">Estado de la organización en el alta (INV-STT-01).</param>
    /// <param name="name">Nombre obligatorio.</param>
    /// <param name="code">Código opcional.</param>
    /// <param name="defaultStartTime">Inicio por defecto opcional.</param>
    /// <param name="defaultEndTime">Fin por defecto opcional.</param>
    /// <returns>Nuevo tipo de turno con identificador generado.</returns>
    public static ShiftType Create(
        Guid organizationId,
        bool organizationIsActive,
        string name,
        string? code,
        TimeOnly? defaultStartTime,
        TimeOnly? defaultEndTime)
    {
        if (organizationId == Guid.Empty)
        {
            throw new DomainException("INV-STT-01", "El tipo de turno requiere una organización válida.");
        }

        if (!organizationIsActive)
        {
            throw new DomainException(
                "INV-STT-01",
                "No se puede crear un tipo de turno en una organización inactiva.");
        }

        var shiftType = new ShiftType
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            IsActive = true
        };

        shiftType.ApplyDetails(name, code, defaultStartTime, defaultEndTime);
        return shiftType;
    }

    #endregion

    #region Behavior

    /// <summary>
    /// Actualiza nombre, código y ventana horaria por defecto.
    /// </summary>
    /// <param name="name">Nuevo nombre obligatorio.</param>
    /// <param name="code">Nuevo código opcional.</param>
    /// <param name="defaultStartTime">Nuevo inicio por defecto opcional.</param>
    /// <param name="defaultEndTime">Nuevo fin por defecto opcional.</param>
    public void Update(string name, string? code, TimeOnly? defaultStartTime, TimeOnly? defaultEndTime) =>
        ApplyDetails(name, code, defaultStartTime, defaultEndTime);

    /// <summary>
    /// Activa o desactiva el tipo de turno.
    /// </summary>
    /// <param name="isActive">Nuevo estado de activación.</param>
    public void SetActive(bool isActive) => IsActive = isActive;

    private void ApplyDetails(
        string name,
        string? code,
        TimeOnly? defaultStartTime,
        TimeOnly? defaultEndTime)
    {
        Name = NormalizeName(name);
        Code = NormalizeCode(code);
        EnsureValidDefaultWindow(defaultStartTime, defaultEndTime);
        DefaultStartTime = defaultStartTime;
        DefaultEndTime = defaultEndTime;
    }

    #endregion

    #region Invariants

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("INV-STT-02", "El nombre del tipo de turno es obligatorio.");
        }

        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException(
                "INV-STT-02",
                $"El nombre del tipo de turno no puede superar {NameMaxLength} caracteres.");
        }

        return trimmed;
    }

    private static string? NormalizeCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        var trimmed = code.Trim();
        if (trimmed.Length > CodeMaxLength)
        {
            throw new DomainException(
                "INV-STT-03",
                $"El código del tipo de turno no puede superar {CodeMaxLength} caracteres.");
        }

        return trimmed;
    }

    private static void EnsureValidDefaultWindow(TimeOnly? start, TimeOnly? end)
    {
        if (start is null || end is null)
        {
            return;
        }

        // INV-STT-04: turnos que cruzan medianoche quedan fuera del MVP (fin debe ser posterior al inicio el mismo día).
        if (end.Value <= start.Value)
        {
            throw new DomainException(
                "INV-STT-04",
                "Si se informan hora de inicio y fin por defecto, el fin debe ser posterior al inicio (sin cruce de medianoche en MVP).");
        }
    }

    #endregion
}
