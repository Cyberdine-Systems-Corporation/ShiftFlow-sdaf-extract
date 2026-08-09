namespace ShiftFlow.Application.Auth;

public static class DemoCredentials
{
    public const string UserName = "demo.admin";
    public const string PasswordConfigurationKey = "Authentication:DemoUser:Password";

    /// <summary>
    /// Solo desarrollo local si no hay user-secrets/env. Documentada en runbook.
    /// </summary>
    public const string DefaultDevelopmentPassword = "ChangeMe!123";
}
