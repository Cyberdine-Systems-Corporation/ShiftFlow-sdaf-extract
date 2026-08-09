using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.Infrastructure.Identity;

/// <summary>
/// Emite y valida tokens de acceso opacos (Data Protection) para clientes BFF
/// como Blazor Server, donde reenviar la cookie Identity es frágil.
/// </summary>
public sealed class AccessTokenService
{
    private const string ProtectorPurpose = "ShiftFlow.AccessToken.v1";
    private static readonly JsonSerializerOptions JsonOptions = new();
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

    private readonly IDataProtector _protector;
    private readonly ILogger<AccessTokenService> _logger;

    /// <summary>
    /// Crea el servicio con el proveedor de Data Protection de la Api.
    /// </summary>
    public AccessTokenService(IDataProtectionProvider dataProtection, ILogger<AccessTokenService> logger)
    {
        _protector = dataProtection.CreateProtector(ProtectorPurpose);
        _logger = logger;
    }

    /// <summary>
    /// Emite un token Bearer para el usuario autenticado.
    /// </summary>
    public string Issue(string userName, IReadOnlyList<string> roles)
    {
        var payload = new TokenPayload(
            userName,
            roles.ToArray(),
            DateTimeOffset.UtcNow.Add(TokenLifetime).ToUnixTimeSeconds());
        var json = JsonSerializer.Serialize(payload, JsonOptions);
        return _protector.Protect(json);
    }

    /// <summary>
    /// Intenta validar un token Bearer y construir el principal.
    /// </summary>
    public ClaimsPrincipal? TryValidate(string token)
    {
        try
        {
            var json = _protector.Unprotect(token);
            var payload = JsonSerializer.Deserialize<TokenPayload>(json, JsonOptions);
            if (payload is null ||
                string.IsNullOrWhiteSpace(payload.UserName) ||
                payload.Exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, payload.UserName),
                new(ClaimTypes.NameIdentifier, payload.UserName)
            };
            foreach (var role in payload.Roles ?? [])
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // authenticationType no nulo ⇒ IsAuthenticated = true
            var identity = new ClaimsIdentity(claims, authenticationType: "ShiftFlowAccessToken");
            return new ClaimsPrincipal(identity);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Token de acceso ShiftFlow inválido.");
            return null;
        }
    }

    private sealed record TokenPayload(string UserName, string[] Roles, long Exp);
}
