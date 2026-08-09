using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Organizations;

public sealed class Organization
{
    public const int NameMaxLength = 200;

    private Organization()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public static Organization Create(string name)
    {
        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = NormalizeName(name),
            IsActive = true
        };
    }

    public void Rename(string name) => Name = NormalizeName(name);

    public void SetActive(bool isActive) => IsActive = isActive;

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("INV-ORG-01", "El nombre de la organización es obligatorio.");
        }

        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException(
                "INV-ORG-01",
                $"El nombre de la organización no puede superar {NameMaxLength} caracteres.");
        }

        return trimmed;
    }
}
