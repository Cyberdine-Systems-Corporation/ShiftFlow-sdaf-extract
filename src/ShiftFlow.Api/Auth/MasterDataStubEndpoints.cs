using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.Api.Auth;

/// <summary>
/// Stub protegido de escritura de maestros (ACC-S1-02) hasta PBI-003.
/// </summary>
public static class MasterDataStubEndpoints
{
    public static IEndpointRouteBuilder MapMasterDataStubEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/organizations", ([FromBody] CreateOrganizationStubRequest? request) =>
            {
                var name = string.IsNullOrWhiteSpace(request?.Name) ? "Organization" : request.Name.Trim();
                return Results.Created($"/api/organizations/{Guid.NewGuid()}", new
                {
                    id = Guid.NewGuid(),
                    name,
                    stub = true,
                    note = "Persistencia real en PBI-003"
                });
            })
            .RequireAuthorization(AuthRoles.Administrator)
            .WithName("CreateOrganizationStub")
            .WithTags("Organizations");

        return endpoints;
    }

    public sealed record CreateOrganizationStubRequest(string? Name);
}
