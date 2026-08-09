namespace ShiftFlow.Application.Auth;

/// <summary>
/// Credenciales y claves de configuración del usuario demo (solo entornos no productivos).
/// </summary>
public static class DemoCredentials
{
    /// <summary>
    /// Nombre de usuario fijo del administrador de demostración.
    /// </summary>
    public const string UserName = "demo.admin";

    /// <summary>
    /// Clave de configuración donde se lee la contraseña del usuario demo.
    /// </summary>
    public const string PasswordConfigurationKey = "Authentication:DemoUser:Password";

    /// <summary>
    /// Solo desarrollo local si no hay user-secrets/env. Documentada en runbook.
    /// </summary>
    public const string DefaultDevelopmentPassword = "ChangeMe!123";
}
