using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ShiftFlow.Web.Auth;

/// <summary>
/// Esquema de autenticación del host Web basado en la sesión BFF en memoria
/// (<see cref="CookieContainerHolder"/>). Necesario porque <c>[Authorize]</c> en páginas Blazor
/// exige <c>HttpContext.User</c> en el GET HTTP (no solo <see cref="ApiAuthenticationStateProvider"/>).
/// </summary>
public sealed class PassThroughAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>Nombre del esquema registrado en DI.</summary>
    public const string SchemeName = "WebHostPassThrough";

    /// <summary>
    /// Inicializa el handler de sesión BFF.
    /// </summary>
    public PassThroughAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <inheritdoc />
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var session = Context.RequestServices.GetRequiredService<CookieContainerHolder>();
        if (!session.HasWebSession || string.IsNullOrWhiteSpace(session.UserName))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, session.UserName),
            new(ClaimTypes.NameIdentifier, session.UserName)
        };
        foreach (var role in session.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, authenticationType: Scheme.Name);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    /// <inheritdoc />
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        // Evitar 401 JSON en navegación de páginas: redirigir al login.
        Response.Redirect("/login");
        return Task.CompletedTask;
    }
}
