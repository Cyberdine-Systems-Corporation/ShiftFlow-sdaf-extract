using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class AuthApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public AuthApiTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ACC_S1_01_Login_demo_con_rol_Administrator()
    {
        HttpClient? client = _factory.CreateClient(new() { HandleCookies = true });

        HttpResponseMessage? login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = DemoCredentials.DefaultDevelopmentPassword
        });

        login.StatusCode.Should().Be(HttpStatusCode.OK);
        AuthUserResponse? body = await login.Content.ReadFromJsonAsync<AuthUserResponse>(JsonOptions);
        body.Should().NotBeNull();
        body!.UserName.Should().Be(DemoCredentials.UserName);
        body.Roles.Should().Contain(AuthRoles.Administrator);

        HttpResponseMessage? me = await client.GetAsync("/api/auth/me");
        me.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ACC_S1_02_anonimo_no_puede_CreateOrganization()
    {
        HttpClient? anonymous = _factory.CreateClient();
        HttpResponseMessage? response = await anonymous.PostAsJsonAsync("/api/organizations", new { name = "Org X" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ACC_S1_07_Logout_invalida_sesion()
    {
        HttpClient? client = _factory.CreateClient(new() { HandleCookies = true });

        HttpResponseMessage? login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = DemoCredentials.DefaultDevelopmentPassword
        });
        login.EnsureSuccessStatusCode();

        HttpResponseMessage? logout = await client.PostAsync("/api/auth/logout", null);
        logout.StatusCode.Should().Be(HttpStatusCode.OK);

        HttpResponseMessage? protectedCall = await client.PostAsJsonAsync("/api/organizations", new { name = "Tras logout" });
        protectedCall.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_invalido_no_autentica()
    {
        HttpClient? client = _factory.CreateClient();
        HttpResponseMessage? response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = "WrongPassword!1"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private sealed record AuthUserResponse(string UserName, string[] Roles);
}
