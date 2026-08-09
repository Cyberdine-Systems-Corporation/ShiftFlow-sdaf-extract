using Microsoft.AspNetCore.Components.Authorization;
using Serilog;
using ShiftFlow.Web.Auth;
using ShiftFlow.Web.Components;

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

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddScoped<CookieContainerHolder>();
    builder.Services.AddScoped<ApiAuthenticationStateProvider>();
    builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
        sp.GetRequiredService<ApiAuthenticationStateProvider>());
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddAuthorizationCore();

    builder.Services.AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri("https+http://api");
        })
        .ConfigurePrimaryHttpMessageHandler(sp =>
        {
            var holder = sp.GetRequiredService<CookieContainerHolder>();
            return new HttpClientHandler
            {
                CookieContainer = holder.Container,
                UseCookies = true
            };
        });

    var app = builder.Build();

    app.MapDefaultEndpoints();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "ShiftFlow.Web terminó de forma inesperada");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
