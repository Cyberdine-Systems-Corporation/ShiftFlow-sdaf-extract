using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class SessionCookieBodyTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly ShiftFlowApiFactory _factory;

    public SessionCookieBodyTests(ShiftFlowApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Login_accessToken_Bearer_autoriza_organizations()
    {
        var loginClient = _factory.CreateClient(new() { HandleCookies = false });
        var login = await loginClient.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = DemoCredentials.DefaultDevelopmentPassword
        });

        login.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await login.Content.ReadFromJsonAsync<LoginBody>(JsonOptions);
        body.Should().NotBeNull();
        body!.AccessToken.Should().NotBeNullOrWhiteSpace();

        var api = _factory.CreateClient(new() { HandleCookies = false });
        var req = new HttpRequestMessage(HttpMethod.Get, "/api/organizations");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", body.AccessToken);
        var orgs = await api.SendAsync(req);
        orgs.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private sealed record LoginBody(string UserName, string[] Roles, string? AccessToken, string[]? SessionCookies);
}
