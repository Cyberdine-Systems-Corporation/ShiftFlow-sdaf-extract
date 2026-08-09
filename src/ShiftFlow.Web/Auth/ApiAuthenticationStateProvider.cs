using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.Web.Auth;

public sealed class ApiAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    : AuthenticationStateProvider
{
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

    public void NotifyChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public bool IsAdministrator(AuthenticationState state) =>
        state.User.IsInRole(AuthRoles.Administrator);

    private sealed record MeResponse(string UserName, string[] Roles);
}
