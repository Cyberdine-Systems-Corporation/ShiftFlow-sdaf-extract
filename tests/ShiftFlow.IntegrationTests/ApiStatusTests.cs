using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class ApiStatusTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _client;

    public ApiStatusTests(ShiftFlowApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetApiStatus_devuelve_ok()
    {
        var response = await _client.GetAsync("/api/status");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<StatusResponse>(JsonOptions);
        payload.Should().NotBeNull();
        payload!.Service.Should().Be("ShiftFlow.Api");
        payload.Status.Should().Be("ok");
    }

    private sealed record StatusResponse(string Service, string Status, string Database);
}
