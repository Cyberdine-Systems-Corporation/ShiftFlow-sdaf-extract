using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.ServiceDiscovery;
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

    // Preferir https; permitir http en perfil local sin certificado (ASPIRE_ALLOW_UNSECURED_TRANSPORT).
    builder.Services.Configure<ServiceDiscoveryOptions>(options =>
    {
        options.AllowedSchemes = ["https", "http"];
    });

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services
        .AddAuthentication(PassThroughAuthenticationHandler.SchemeName)
        .AddScheme<AuthenticationSchemeOptions, PassThroughAuthenticationHandler>(
            PassThroughAuthenticationHandler.SchemeName,
            _ => { });
    builder.Services.AddAuthorization();

    builder.Services.AddSingleton<CookieContainerHolder>();
    builder.Services.AddTransient<PropagateAllCookiesHandler>();
    builder.Services.AddScoped<ApiAuthenticationStateProvider>();
    builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
        sp.GetRequiredService<ApiAuthenticationStateProvider>());
    builder.Services.AddScoped<ShiftFlow.Web.Api.MastersApiClient>();
    builder.Services.AddCascadingAuthenticationState();

    builder.Services.AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri("https+http://api");
        })
#pragma warning disable EXTEXP0001
        .RemoveAllResilienceHandlers()
#pragma warning restore EXTEXP0001
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            // Crítico: UseCookies=true pisa/ignora la cabecera Cookie manual del Propagate handler.
            UseCookies = false,
            AllowAutoRedirect = false
        })
        .AddHttpMessageHandler<PropagateAllCookiesHandler>();

    var app = builder.Build();

    app.MapDefaultEndpoints();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
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
