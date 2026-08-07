using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ShiftFlow.IntegrationTests;

public class ApiStatusTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly WebApplicationFactory<Program> _factory;

    public ApiStatusTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetApiStatus_devuelve_ok()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/status");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<StatusResponse>(JsonOptions);
        payload.Should().NotBeNull();
        payload!.Service.Should().Be("ShiftFlow.Api");
        payload.Status.Should().Be("ok");
    }

    private sealed record StatusResponse(string Service, string Status, string Database);
}
