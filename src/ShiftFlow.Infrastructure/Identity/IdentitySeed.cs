using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftFlow.Application.Auth;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Identity;

/// <summary>
/// Provisiona esquema, rol Administrator y usuario demo en desarrollo.
/// </summary>
public static class IdentitySeed
{
    /// <summary>
    /// Asegura la base de datos, el rol Administrator y el usuario demo si no existen.
    /// </summary>
    /// <param name="services">Proveedor raíz de servicios (se crea un scope interno).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var sp = scope.ServiceProvider;
        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeed");
        var db = sp.GetRequiredService<ShiftFlowDbContext>();
        var configuration = sp.GetRequiredService<IConfiguration>();

        await db.Database.EnsureCreatedAsync(cancellationToken);

        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(AuthRoles.Administrator))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(AuthRoles.Administrator));
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"No se pudo crear el rol {AuthRoles.Administrator}: {FormatErrors(roleResult)}");
            }
        }

        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var existing = await userManager.FindByNameAsync(DemoCredentials.UserName);
        if (existing is not null)
        {
            return;
        }

        var password = configuration[DemoCredentials.PasswordConfigurationKey];
        if (string.IsNullOrWhiteSpace(password))
        {
            password = DemoCredentials.DefaultDevelopmentPassword;
            logger.LogWarning(
                "No hay {Key} configurada; se usa la contraseña de desarrollo por defecto. Sobrescribe con user-secrets o env.",
                DemoCredentials.PasswordConfigurationKey);
        }

        var user = new ApplicationUser
        {
            UserName = DemoCredentials.UserName,
            Email = "demo.admin@shiftflow.local",
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"No se pudo crear el usuario demo: {FormatErrors(createResult)}");
        }

        var addRole = await userManager.AddToRoleAsync(user, AuthRoles.Administrator);
        if (!addRole.Succeeded)
        {
            throw new InvalidOperationException(
                $"No se pudo asignar el rol Administrator: {FormatErrors(addRole)}");
        }

        logger.LogInformation(
            "Usuario demo {User} provisionado con rol {Role}.",
            DemoCredentials.UserName,
            AuthRoles.Administrator);
    }

    private static string FormatErrors(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(e => e.Description));
}
