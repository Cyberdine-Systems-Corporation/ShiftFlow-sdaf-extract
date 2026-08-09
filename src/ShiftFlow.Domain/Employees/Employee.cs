using System.Net.Mail;
using System.Text.RegularExpressions;
using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Employees;

public sealed class Employee
{
    public const int DisplayNameMaxLength = 200;
    public const int EmailMaxLength = 320;

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private Employee()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public Guid DepartmentId { get; private set; }

    public string DisplayName { get; private set; } = string.Empty;

    public string? Email { get; private set; }

    public bool IsActive { get; private set; }

    public static Employee Create(
        Guid organizationId,
        Guid departmentId,
        Guid departmentOrganizationId,
        bool departmentIsActive,
        string displayName,
        string? email)
    {
        EnsureDepartmentBelongsToOrganization(organizationId, departmentId, departmentOrganizationId);

        if (!departmentIsActive)
        {
            throw new DomainException(
                "INV-EMP-01",
                "No se puede crear un empleado en un departamento inactivo.");
        }

        return new Employee
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            DepartmentId = departmentId,
            DisplayName = NormalizeDisplayName(displayName),
            Email = NormalizeEmail(email),
            IsActive = true
        };
    }

    public void Update(
        Guid departmentId,
        Guid departmentOrganizationId,
        string displayName,
        string? email)
    {
        EnsureDepartmentBelongsToOrganization(OrganizationId, departmentId, departmentOrganizationId);
        DepartmentId = departmentId;
        DisplayName = NormalizeDisplayName(displayName);
        Email = NormalizeEmail(email);
    }

    public void SetActive(bool isActive) => IsActive = isActive;

    private static void EnsureDepartmentBelongsToOrganization(
        Guid organizationId,
        Guid departmentId,
        Guid departmentOrganizationId)
    {
        if (organizationId == Guid.Empty || departmentId == Guid.Empty)
        {
            throw new DomainException(
                "INV-EMP-01",
                "El empleado requiere organización y departamento válidos.");
        }

        if (departmentOrganizationId != organizationId)
        {
            throw new DomainException(
                "INV-EMP-01",
                "El departamento del empleado debe pertenecer a la misma organización.");
        }
    }

    private static string NormalizeDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new DomainException("INV-EMP-02", "El nombre visible del empleado es obligatorio.");
        }

        var trimmed = displayName.Trim();
        if (trimmed.Length > DisplayNameMaxLength)
        {
            throw new DomainException(
                "INV-EMP-02",
                $"El nombre visible no puede superar {DisplayNameMaxLength} caracteres.");
        }

        return trimmed;
    }

    private static string? NormalizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var trimmed = email.Trim();
        if (trimmed.Length > EmailMaxLength || !EmailRegex.IsMatch(trimmed))
        {
            throw new DomainException("INV-EMP-01", "El email del empleado no tiene un formato válido.");
        }

        // Validación adicional con MailAddress (rechaza casos límite raros).
        try
        {
            _ = new MailAddress(trimmed);
        }
        catch (FormatException)
        {
            throw new DomainException("INV-EMP-01", "El email del empleado no tiene un formato válido.");
        }

        return trimmed;
    }
}
