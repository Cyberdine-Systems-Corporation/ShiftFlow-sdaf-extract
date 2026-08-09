using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Departments;

public sealed class Department
{
    public const int NameMaxLength = 200;

    private Department()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

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

    public void Rename(string name) => Name = NormalizeName(name);

    public void SetActive(bool isActive) => IsActive = isActive;

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
}
