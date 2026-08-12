using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Auth;
using ShiftFlow.Application.Common;
using ShiftFlow.Application.ShiftAssignments;
using ShiftFlow.Domain.Common;

namespace ShiftFlow.Api.Scheduling;

/// <summary>
/// Endpoints HTTP de calendario y asignación manual de turnos (PBI-005).
/// </summary>
public static class SchedulingEndpoints
{
    #region Endpoints

    /// <summary>
    /// Registra las rutas de planificación bajo <c>/api</c> (rol Administrator).
    /// </summary>
    /// <param name="endpoints">Builder de rutas de la aplicación.</param>
    /// <returns>El mismo <paramref name="endpoints"/> para encadenar.</returns>
    public static IEndpointRouteBuilder MapSchedulingEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var orgs = endpoints.MapGroup("/api/organizations")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Calendar");

        orgs.MapGet("/{organizationId:guid}/calendar", GetMonthCalendarAsync)
            .WithName("GetMonthCalendar");

        orgs.MapPost("/{organizationId:guid}/assignments", AssignShiftAsync)
            .WithName("AssignShift")
            .WithTags("Assignments");

        var assignments = endpoints.MapGroup("/api/assignments")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Assignments");

        assignments.MapPost("/{id:guid}/cancel", CancelShiftAsync)
            .WithName("CancelShift");

        return endpoints;
    }

    private static Task<IResult> GetMonthCalendarAsync(
        Guid organizationId,
        [FromQuery] int year,
        [FromQuery] int month,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new GetMonthCalendarQuery(organizationId, year, month), cancellationToken),
            Results.Ok);

    private static Task<IResult> AssignShiftAsync(
        Guid organizationId,
        [FromBody] AssignShiftBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(
                new AssignShiftCommand(
                    organizationId,
                    body.EmployeeId,
                    body.ShiftTypeId,
                    body.StartAt,
                    body.EndAt),
                cancellationToken),
            dto => Results.Created($"/api/assignments/{dto.Id}", dto));

    private static Task<IResult> CancelShiftAsync(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new CancelShiftCommand(id), cancellationToken),
            Results.Ok);

    private static async Task<IResult> ExecuteAsync<T>(Func<Task<T>> action, Func<T, IResult> onSuccess)
    {
        try
        {
            var result = await action();
            return onSuccess(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return Results.BadRequest(new { error = ex.Message, code = "INV-CAL-01" });
        }
        catch (DomainException ex)
        {
            // INV-ASN-* estructurales y HR-01… se distinguen por el código en el cuerpo.
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
    /// Cuerpo de alta de asignación de turno.
    /// </summary>
    /// <param name="EmployeeId">Empleado destino.</param>
    /// <param name="ShiftTypeId">Tipo de turno.</param>
    /// <param name="StartAt">Inicio del intervalo.</param>
    /// <param name="EndAt">Fin del intervalo.</param>
    public sealed record AssignShiftBody(
        Guid EmployeeId,
        Guid ShiftTypeId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt);

    #endregion
}
