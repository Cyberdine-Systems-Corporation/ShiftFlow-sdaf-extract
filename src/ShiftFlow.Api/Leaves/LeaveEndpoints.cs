using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Auth;
using ShiftFlow.Application.Common;
using ShiftFlow.Application.Leaves;
using ShiftFlow.Domain.Common;

namespace ShiftFlow.Api.Leaves;

/// <summary>
/// Endpoints HTTP de ausencias / Leave (PBI-007).
/// </summary>
public static class LeaveEndpoints
{
    #region Endpoints

    /// <summary>
    /// Registra las rutas de Leave bajo <c>/api</c> (rol Administrator).
    /// </summary>
    /// <param name="endpoints">Builder de rutas de la aplicación.</param>
    /// <returns>El mismo <paramref name="endpoints"/> para encadenar.</returns>
    public static IEndpointRouteBuilder MapLeaveEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder? orgs = endpoints.MapGroup("/api/organizations")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Leaves");

        orgs.MapGet("/{organizationId:guid}/leaves", ListLeavesAsync)
            .WithName("ListLeaves");

        orgs.MapPost("/{organizationId:guid}/leaves", RegisterLeaveAsync)
            .WithName("RegisterLeave");

        RouteGroupBuilder? leaves = endpoints.MapGroup("/api/leaves")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Leaves");

        leaves.MapPost("/{id:guid}/cancel", CancelLeaveAsync)
            .WithName("CancelLeave");

        return endpoints;
    }

    private static Task<IResult> ListLeavesAsync(
        Guid organizationId,
        [FromQuery] Guid? employeeId,
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] bool? activeOnly,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(
                new ListLeavesQuery(
                    organizationId,
                    employeeId,
                    year,
                    month,
                    activeOnly ?? true),
                cancellationToken),
            Results.Ok);

    private static Task<IResult> RegisterLeaveAsync(
        Guid organizationId,
        [FromBody] RegisterLeaveBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(
                new RegisterLeaveCommand(
                    organizationId,
                    body.EmployeeId,
                    body.StartOn,
                    body.EndOn,
                    body.Kind,
                    body.Reason),
                cancellationToken),
            dto => Results.Created($"/api/leaves/{dto.Id}", dto));

    private static Task<IResult> CancelLeaveAsync(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new CancelLeaveCommand(id), cancellationToken),
            Results.Ok);

    private static async Task<IResult> ExecuteAsync<T>(Func<Task<T>> action, Func<T, IResult> onSuccess)
    {
        try
        {
            T? result = await action();
            return onSuccess(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return Results.BadRequest(new { error = ex.Message, code = "INV-LEA-RANGE" });
        }
        catch (DomainException ex)
        {
            return Results.BadRequest(new { error = ex.Message, code = ex.Code });
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { error = ex.Message });
        }
    }

    #endregion

    #region Contracts

    /// <summary>
    /// Cuerpo de alta de leave.
    /// </summary>
    /// <param name="EmployeeId">Empleado ausente.</param>
    /// <param name="StartOn">Inicio inclusive.</param>
    /// <param name="EndOn">Fin inclusive.</param>
    /// <param name="Kind">Tipo opcional.</param>
    /// <param name="Reason">Motivo opcional.</param>
    public sealed record RegisterLeaveBody(
        Guid EmployeeId,
        DateOnly StartOn,
        DateOnly EndOn,
        string? Kind,
        string? Reason);

    #endregion
}
