using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShiftFlow.Application.Auth;
using ShiftFlow.Infrastructure.Identity;

namespace ShiftFlow.Api.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", LoginAsync)
            .AllowAnonymous()
            .WithName("Login");

        group.MapPost("/logout", LogoutAsync)
            .RequireAuthorization()
            .WithName("Logout");

        group.MapGet("/me", Me)
            .RequireAuthorization()
            .WithName("Me");

        return endpoints;
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { error = "UserName y Password son obligatorios." });
        }

        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var result = await signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        return Results.Ok(new AuthUserResponse(user.UserName!, roles.ToArray()));
    }

    private static async Task<IResult> LogoutAsync(SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok(new { status = "logged_out" });
    }

    private static IResult Me(ClaimsPrincipal principal)
    {
        var userName = principal.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
        {
            return Results.Unauthorized();
        }

        var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        return Results.Ok(new AuthUserResponse(userName, roles));
    }

    public sealed record LoginRequest(string UserName, string Password);

    public sealed record AuthUserResponse(string UserName, string[] Roles);
}
