using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ShiftFlow.Infrastructure.Identity;

/// <summary>
/// Middleware que autentica cabecera <c>Authorization: Bearer</c> emitida por <see cref="AccessTokenService"/>.
/// Se ejecuta tras la cookie Identity; solo actúa si aún no hay usuario autenticado.
/// </summary>
public static class AccessTokenMiddleware
{
    private const string BearerPrefix = "Bearer ";

    /// <summary>
    /// Registra el middleware de token Bearer para clientes BFF (Blazor Web → Api).
    /// </summary>
    /// <param name="app">Pipeline de la aplicación.</param>
    /// <returns>El mismo <paramref name="app"/> para encadenar.</returns>
    public static IApplicationBuilder UseShiftFlowAccessTokens(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                var header = context.Request.Headers.Authorization.ToString();
                if (header.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var token = header[BearerPrefix.Length..].Trim();
                    if (token.Length > 0)
                    {
                        var tokens = context.RequestServices.GetRequiredService<AccessTokenService>();
                        var principal = tokens.TryValidate(token);
                        if (principal is not null)
                        {
                            context.User = principal;
                        }
                    }
                }
            }

            await next();
        });
    }
}
