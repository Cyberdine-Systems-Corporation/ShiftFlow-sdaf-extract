using Serilog;
using ShiftFlow.Api.Auth;
using ShiftFlow.Api.Masters;
using ShiftFlow.Api.Scheduling;
using ShiftFlow.Application;
using ShiftFlow.Infrastructure;
using ShiftFlow.Infrastructure.Identity;
using ShiftFlow.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();


try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.AddServiceDefaults();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddOpenApi();

    var app = builder.Build();

    await IdentitySeed.InitializeAsync(app.Services);

    app.MapDefaultEndpoints();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseShiftFlowAccessTokens();
    app.UseAuthorization();

    app.MapGet("/api/status", async (ShiftFlowDbContext db, CancellationToken cancellationToken) =>
        {
            var canConnect = await db.Database.CanConnectAsync(cancellationToken);
            return Results.Ok(new
            {
                service = "ShiftFlow.Api",
                status = "ok",
                database = canConnect ? "reachable" : "unreachable"
            });
        })
        .AllowAnonymous()
        .WithName("GetApiStatus");

    app.MapAuthEndpoints();
    app.MapMasterDataEndpoints();
    app.MapSchedulingEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "ShiftFlow.Api terminó de forma inesperada");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Punto de entrada parcial de la Api (visible para tests de integración).
/// </summary>
public partial class Program;
