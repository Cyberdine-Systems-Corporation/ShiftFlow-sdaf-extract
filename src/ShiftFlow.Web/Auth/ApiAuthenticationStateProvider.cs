using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.Web.Auth;

/// <summary>
/// Proveedor de estado de autenticación basado en <c>/api/auth/me</c> y cookies de la Api.
/// </summary>
/// <param name="httpClientFactory">Fábrica del cliente HTTP nombrado <c>api</c>.</param>
public sealed class ApiAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    : AuthenticationStateProvider
{
    /// <summary>
    /// Obtiene el estado actual consultando la Api; si falla, devuelve usuario anónimo.
    /// </summary>
    /// <returns>Estado de autenticación con claims de nombre y roles, o identidad vacía.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var client = httpClientFactory.CreateClient("api");
            using var response = await client.GetAsync("/api/auth/me");
            if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var me = await response.Content.ReadFromJsonAsync<MeResponse>();
            if (me is null || string.IsNullOrWhiteSpace(me.UserName))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, me.UserName),
                new(ClaimTypes.NameIdentifier, me.UserName)
            };
            foreach (var role in me.Roles ?? [])
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, authenticationType: "ApiCookie");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    /// <summary>
    /// Notifica a Blazor que el estado de autenticación debe recalcularse.
    /// </summary>
    public void NotifyChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    /// <summary>
    /// Indica si el usuario del estado pertenece al rol Administrator.
    /// </summary>
    /// <param name="state">Estado de autenticación actual.</param>
    /// <returns><see langword="true"/> si tiene el rol Administrator.</returns>
    public bool IsAdministrator(AuthenticationState state) =>
        state.User.IsInRole(AuthRoles.Administrator);

    private sealed record MeResponse(string UserName, string[] Roles);
}
