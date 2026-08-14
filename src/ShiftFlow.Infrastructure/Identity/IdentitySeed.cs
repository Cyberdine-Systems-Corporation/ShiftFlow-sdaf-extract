using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftFlow.Application.Auth;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure.Identity;

/// <summary>
/// Provisiona esquema (ADR-007), rol Administrator, usuario demo y catálogo de vitrina (PBI-010).
/// </summary>
public static class IdentitySeed
{
    /// <summary>
    /// Aplica el esquema, provisiona Identity de demo y, si aplica, el catálogo de casuísticas.
    /// </summary>
    /// <param name="services">Proveedor raíz de servicios (se crea un scope interno).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = services.CreateAsyncScope();
        IServiceProvider? sp = scope.ServiceProvider;
        ILogger? logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeed");
        ShiftFlowDbContext? db = sp.GetRequiredService<ShiftFlowDbContext>();
        IConfiguration? configuration = sp.GetRequiredService<IConfiguration>();

        await DatabaseInitializer.EnsureSchemaAsync(db, logger, cancellationToken);

        RoleManager<IdentityRole>? roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(AuthRoles.Administrator))
        {
            IdentityResult? roleResult = await roleManager.CreateAsync(new IdentityRole(AuthRoles.Administrator));
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"No se pudo crear el rol {AuthRoles.Administrator}: {FormatErrors(roleResult)}");
            }
        }

        UserManager<ApplicationUser>? userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? existing = await userManager.FindByNameAsync(DemoCredentials.UserName);
        if (existing is null)
        {
            string? password = configuration[DemoCredentials.PasswordConfigurationKey];
            if (string.IsNullOrWhiteSpace(password))
            {
                password = DemoCredentials.DefaultDevelopmentPassword;
                logger.LogWarning(
                    "No hay {Key} configurada; se usa la contraseña de desarrollo por defecto. Sobrescribe con user-secrets o env.",
                    DemoCredentials.PasswordConfigurationKey);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = DemoCredentials.UserName,
                Email = "demo.admin@shiftflow.local",
                EmailConfirmed = true
            };

            IdentityResult? createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"No se pudo crear el usuario demo: {FormatErrors(createResult)}");
            }

            IdentityResult? addRole = await userManager.AddToRoleAsync(user, AuthRoles.Administrator);
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

        await DemoCatalogSeed.EnsureAsync(db, configuration, logger, cancellationToken);
    }

    private static string FormatErrors(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(e => e.Description));
}
