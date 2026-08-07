using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftFlow.Infrastructure.Persistence;

namespace ShiftFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("shiftflow")
            ?? configuration.GetConnectionString("ShiftFlow")
            ?? "Host=localhost;Port=5432;Database=shiftflow;Username=shiftflow;Password=shiftflow";

        services.AddDbContext<ShiftFlowDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
