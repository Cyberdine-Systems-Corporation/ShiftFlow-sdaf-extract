using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.ShiftTypes;

public sealed class ShiftType
{
    public const int NameMaxLength = 200;
    public const int CodeMaxLength = 50;

    private ShiftType()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Code { get; private set; }

    public TimeOnly? DefaultStartTime { get; private set; }

    public TimeOnly? DefaultEndTime { get; private set; }

    public bool IsActive { get; private set; }

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

    public void Update(string name, string? code, TimeOnly? defaultStartTime, TimeOnly? defaultEndTime) =>
        ApplyDetails(name, code, defaultStartTime, defaultEndTime);

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

        if (end.Value <= start.Value)
        {
            throw new DomainException(
                "INV-STT-04",
                "Si se informan hora de inicio y fin por defecto, el fin debe ser posterior al inicio (sin cruce de medianoche en MVP).");
        }
    }
}
