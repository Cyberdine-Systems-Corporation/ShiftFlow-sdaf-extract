using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ShiftFlow.Application;

/// <summary>
/// Registro de servicios de la capa Application en el contenedor DI.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra MediatR y handlers del ensamblado Application.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación anfitriona.</param>
    /// <returns>La misma colección para encadenar registros.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
