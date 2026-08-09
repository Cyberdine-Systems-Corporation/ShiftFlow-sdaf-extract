using System.Net.Mail;
using System.Text.RegularExpressions;
using ShiftFlow.Domain.Common;

namespace ShiftFlow.Domain.Employees;

/// <summary>
/// Agregado de empleado asignado a un departamento de una organización.
/// </summary>
public sealed class Employee
{
    /// <summary>
    /// Longitud máxima del nombre visible (INV-EMP-02).
    /// </summary>
    public const int DisplayNameMaxLength = 200;

    /// <summary>
    /// Longitud máxima del email opcional.
    /// </summary>
    public const int EmailMaxLength = 320;

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private Employee()
    {
    }

    /// <summary>
    /// Identificador del empleado.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Organización a la que pertenece el empleado.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Departamento asignado (debe pertenecer a la misma organización).
    /// </summary>
    public Guid DepartmentId { get; private set; }

    /// <summary>
    /// Nombre visible normalizado (trim).
    /// </summary>
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>
    /// Email opcional normalizado; <c>null</c> si no se informa.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Indica si el empleado está activo.
    /// </summary>
    public bool IsActive { get; private set; }

    #region Factory

    /// <summary>
    /// Crea un empleado activo en un departamento activo de la misma organización.
    /// </summary>
    /// <param name="organizationId">Organización del empleado.</param>
    /// <param name="departmentId">Departamento de asignación.</param>
    /// <param name="departmentOrganizationId">Organización real del departamento (INV-EMP-01).</param>
    /// <param name="departmentIsActive">Estado del departamento en el alta.</param>
    /// <param name="displayName">Nombre visible obligatorio.</param>
    /// <param name="email">Email opcional; vacío se trata como ausente.</param>
    /// <returns>Nuevo empleado con identificador generado.</returns>
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

    #endregion

    #region Behavior

    /// <summary>
    /// Actualiza departamento, nombre visible y email manteniendo la coherencia organizativa.
    /// </summary>
    /// <param name="departmentId">Nuevo departamento.</param>
    /// <param name="departmentOrganizationId">Organización real del departamento destino.</param>
    /// <param name="displayName">Nuevo nombre visible.</param>
    /// <param name="email">Nuevo email opcional.</param>
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

    /// <summary>
    /// Activa o desactiva el empleado.
    /// </summary>
    /// <param name="isActive">Nuevo estado de activación.</param>
    public void SetActive(bool isActive) => IsActive = isActive;

    #endregion

    #region Invariants

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

    #endregion
}
