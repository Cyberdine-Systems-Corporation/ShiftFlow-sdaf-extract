using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.IntegrationTests;

/// <summary>
/// El catálogo de demo no debe mezclarse con fixtures SQLite (handbook 16).
/// </summary>
[Collection(nameof(ApiCollection))]
public class DemoCatalogSeedTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public DemoCatalogSeedTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Catalogo_demo_no_se_siembra_en_sqlite()
    {
        HttpClient client = _factory.CreateClient(new() { HandleCookies = true });
        HttpResponseMessage login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = DemoCredentials.DefaultDevelopmentPassword
        });
        login.StatusCode.Should().Be(HttpStatusCode.OK);

        List<OrganizationResponse>? orgs = await client.GetFromJsonAsync<List<OrganizationResponse>>(
            "/api/organizations",
            JsonOptions);

        orgs.Should().NotBeNull();
        orgs.Should().NotContain(organization => organization.Name == DemoCatalogSeed.OperationOrganizationName);
        orgs.Should().NotContain(organization => organization.Name == DemoCatalogSeed.RestOrganizationName);
    }

    private sealed record OrganizationResponse(Guid Id, string Name, bool IsActive, int MinimumRestMinutes);
}
