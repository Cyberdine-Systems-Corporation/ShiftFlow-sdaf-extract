using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Auth;
using ShiftFlow.Application.Common;
using ShiftFlow.Application.Departments;
using ShiftFlow.Application.Employees;
using ShiftFlow.Application.Organizations;
using ShiftFlow.Domain.Common;

namespace ShiftFlow.Api.Masters;

public static class MasterDataEndpoints
{
    public static IEndpointRouteBuilder MapMasterDataEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var orgs = endpoints.MapGroup("/api/organizations")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Organizations");

        orgs.MapPost("/", CreateOrganizationAsync).WithName("CreateOrganization");
        orgs.MapGet("/", ListOrganizationsAsync).WithName("ListOrganizations");
        orgs.MapGet("/{id:guid}", GetOrganizationAsync).WithName("GetOrganizationById");
        orgs.MapPut("/{id:guid}/name", RenameOrganizationAsync).WithName("RenameOrganization");
        orgs.MapPut("/{id:guid}/active", SetOrganizationActiveAsync).WithName("SetOrganizationActive");

        orgs.MapPost("/{organizationId:guid}/departments", CreateDepartmentAsync)
            .WithName("CreateDepartment")
            .WithTags("Departments");
        orgs.MapGet("/{organizationId:guid}/departments", ListDepartmentsAsync)
            .WithName("ListDepartmentsByOrganization")
            .WithTags("Departments");

        orgs.MapPost("/{organizationId:guid}/employees", CreateEmployeeAsync)
            .WithName("CreateEmployee")
            .WithTags("Employees");
        orgs.MapGet("/{organizationId:guid}/employees", ListEmployeesByOrganizationAsync)
            .WithName("ListEmployeesByOrganization")
            .WithTags("Employees");

        var departments = endpoints.MapGroup("/api/departments")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Departments");

        departments.MapPut("/{id:guid}/name", RenameDepartmentAsync).WithName("RenameDepartment");
        departments.MapPut("/{id:guid}/active", SetDepartmentActiveAsync).WithName("SetDepartmentActive");
        departments.MapGet("/{departmentId:guid}/employees", ListEmployeesByDepartmentAsync)
            .WithName("ListEmployeesByDepartment")
            .WithTags("Employees");

        var employees = endpoints.MapGroup("/api/employees")
            .RequireAuthorization(AuthRoles.Administrator)
            .WithTags("Employees");

        employees.MapPut("/{id:guid}", UpdateEmployeeAsync).WithName("UpdateEmployee");
        employees.MapPut("/{id:guid}/active", SetEmployeeActiveAsync).WithName("SetEmployeeActive");

        return endpoints;
    }

    private static Task<IResult> CreateOrganizationAsync(
        [FromBody] NameBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(() => mediator.Send(new CreateOrganizationCommand(body.Name ?? string.Empty), cancellationToken),
            dto => Results.Created($"/api/organizations/{dto.Id}", dto));

    private static Task<IResult> ListOrganizationsAsync(IMediator mediator, CancellationToken cancellationToken) =>
        ExecuteAsync(() => mediator.Send(new ListOrganizationsQuery(), cancellationToken), Results.Ok);

    private static Task<IResult> GetOrganizationAsync(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(() => mediator.Send(new GetOrganizationByIdQuery(id), cancellationToken), Results.Ok);

    private static Task<IResult> RenameOrganizationAsync(
        Guid id,
        [FromBody] NameBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new RenameOrganizationCommand(id, body.Name ?? string.Empty), cancellationToken),
            Results.Ok);

    private static Task<IResult> SetOrganizationActiveAsync(
        Guid id,
        [FromBody] ActiveBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new SetOrganizationActiveCommand(id, body.IsActive), cancellationToken),
            Results.Ok);

    private static Task<IResult> CreateDepartmentAsync(
        Guid organizationId,
        [FromBody] NameBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(
                new CreateDepartmentCommand(organizationId, body.Name ?? string.Empty),
                cancellationToken),
            dto => Results.Created($"/api/departments/{dto.Id}", dto));

    private static Task<IResult> ListDepartmentsAsync(
        Guid organizationId,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new ListDepartmentsByOrganizationQuery(organizationId), cancellationToken),
            Results.Ok);

    private static Task<IResult> RenameDepartmentAsync(
        Guid id,
        [FromBody] NameBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new RenameDepartmentCommand(id, body.Name ?? string.Empty), cancellationToken),
            Results.Ok);

    private static Task<IResult> SetDepartmentActiveAsync(
        Guid id,
        [FromBody] ActiveBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new SetDepartmentActiveCommand(id, body.IsActive), cancellationToken),
            Results.Ok);

    private static Task<IResult> CreateEmployeeAsync(
        Guid organizationId,
        [FromBody] CreateEmployeeBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(
                new CreateEmployeeCommand(
                    organizationId,
                    body.DepartmentId,
                    body.DisplayName ?? string.Empty,
                    body.Email),
                cancellationToken),
            dto => Results.Created($"/api/employees/{dto.Id}", dto));

    private static Task<IResult> ListEmployeesByOrganizationAsync(
        Guid organizationId,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new ListEmployeesByOrganizationQuery(organizationId), cancellationToken),
            Results.Ok);

    private static Task<IResult> ListEmployeesByDepartmentAsync(
        Guid departmentId,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new ListEmployeesByDepartmentQuery(departmentId), cancellationToken),
            Results.Ok);

    private static Task<IResult> UpdateEmployeeAsync(
        Guid id,
        [FromBody] UpdateEmployeeBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(
                new UpdateEmployeeCommand(
                    id,
                    body.DepartmentId,
                    body.DisplayName ?? string.Empty,
                    body.Email),
                cancellationToken),
            Results.Ok);

    private static Task<IResult> SetEmployeeActiveAsync(
        Guid id,
        [FromBody] ActiveBody body,
        IMediator mediator,
        CancellationToken cancellationToken) =>
        ExecuteAsync(
            () => mediator.Send(new SetEmployeeActiveCommand(id, body.IsActive), cancellationToken),
            Results.Ok);

    private static async Task<IResult> ExecuteAsync<T>(Func<Task<T>> action, Func<T, IResult> onSuccess)
    {
        try
        {
            var result = await action();
            return onSuccess(result);
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

    public sealed record NameBody(string? Name);

    public sealed record ActiveBody(bool IsActive);

    public sealed record CreateEmployeeBody(Guid DepartmentId, string? DisplayName, string? Email);

    public sealed record UpdateEmployeeBody(Guid DepartmentId, string? DisplayName, string? Email);
}
