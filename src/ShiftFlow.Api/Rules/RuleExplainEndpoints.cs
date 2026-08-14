using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Auth;
using ShiftFlow.Application.Rules;

namespace ShiftFlow.Api.Rules;

/// <summary>
/// Endpoints HTTP del stub de explicación de reglas (PBI-011).
/// </summary>
public static class RuleExplainEndpoints
{
    #region Endpoints

    /// <summary>
    /// Registra <c>GET /api/rules/explain</c> (rol Administrator).
    /// </summary>
    /// <param name="endpoints">Builder de rutas de la aplicación.</param>
    /// <returns>El mismo <paramref name="endpoints"/> para encadenar.</returns>
    public static IEndpointRouteBuilder MapRuleExplainEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder? rules = endpoints.MapGroup("/api/rules")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Rules");

        rules.MapGet("/explain", ExplainRuleAsync)
            .WithName("ExplainRule");

        return endpoints;
    }

    private static async Task<IResult> ExplainRuleAsync(
        [FromQuery] string? code,
        [FromQuery] Guid? organizationId,
        [FromQuery] Guid? employeeId,
        [FromQuery] DateTimeOffset? startAt,
        [FromQuery] DateTimeOffset? endAt,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            RuleExplanation explanation = await mediator.Send(
                new ExplainRuleQuery(code ?? string.Empty, organizationId, employeeId, startAt, endAt),
                cancellationToken);
            return Results.Ok(explanation);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message, code = "INV-RUL-01" });
        }
    }

    #endregion
}
