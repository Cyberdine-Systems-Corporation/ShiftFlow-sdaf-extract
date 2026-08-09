using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftFlow.Application.Auth;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftTypes;
using ShiftFlow.Infrastructure.Identity;
using ShiftFlow.Infrastructure.Persistence;
using ShiftFlow.Infrastructure.Persistence.Repositories;

// AddIdentity / cookie viven en Microsoft.Extensions.DependencyInjection (FrameworkReference AspNetCore.App).


namespace ShiftFlow.Infrastructure;

/// <summary>
/// Registro de servicios de la capa Infrastructure en el contenedor DI.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra persistencia EF Core, repositorios, Identity y políticas de autorización.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación anfitriona.</param>
    /// <param name="configuration">Configuración (cadena de conexión e Identity).</param>
    /// <returns>La misma colección para encadenar registros.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Persistence

        var connectionString = configuration.GetConnectionString("shiftflow")
            ?? configuration.GetConnectionString("ShiftFlow")
            ?? "Host=localhost;Port=5432;Database=shiftflow;Username=shiftflow;Password=shiftflow";

        services.AddDbContext<ShiftFlowDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ShiftFlowDbContext>());
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IShiftTypeRepository, ShiftTypeRepository>();

        #endregion

        #region Auth

        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<ShiftFlowDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "ShiftFlow.Auth";
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
            options.SlidingExpiration = true;
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthRoles.Administrator, policy =>
                policy.RequireRole(AuthRoles.Administrator));
        });

        services.AddSingleton<AccessTokenService>();

        #endregion

        return services;
    }
}
