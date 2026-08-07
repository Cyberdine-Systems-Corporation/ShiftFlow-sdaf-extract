using Serilog;
using ShiftFlow.Application;
using ShiftFlow.Infrastructure;
using ShiftFlow.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

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

    app.MapDefaultEndpoints();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

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
        .WithName("GetApiStatus");

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

public partial class Program;
