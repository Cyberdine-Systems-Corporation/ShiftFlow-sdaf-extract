using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ShiftFlow.Infrastructure.Persistence;

/// <summary>
/// Aplica el esquema según el provider: migraciones EF en PostgreSQL, <c>EnsureCreated</c> solo en SQLite de tests (ADR-007).
/// </summary>
public static class DatabaseInitializer
{
    private const string SqliteProviderToken = "Sqlite";

    /// <summary>
    /// Crea o actualiza el esquema de <paramref name="db"/>.
    /// </summary>
    /// <param name="db">Contexto ya resuelto en un scope.</param>
    /// <param name="logger">Registro del camino elegido (Migrate vs EnsureCreated).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public static async Task EnsureSchemaAsync(
        ShiftFlowDbContext db,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        if (IsSqlite(db))
        {
            logger.LogInformation("Provider SQLite: se usa EnsureCreated (tests; ADR-007).");
            await db.Database.EnsureCreatedAsync(cancellationToken);
            return;
        }

        logger.LogInformation("Provider {Provider}: se aplican migraciones EF Core.", db.Database.ProviderName);
        await db.Database.MigrateAsync(cancellationToken);
    }

    /// <summary>
    /// Indica si el contexto usa el provider SQLite (tests de integración).
    /// </summary>
    /// <param name="db">Contexto a inspeccionar.</param>
    /// <returns><see langword="true"/> si el nombre de provider contiene Sqlite.</returns>
    public static bool IsSqlite(ShiftFlowDbContext db)
    {
        string? provider = db.Database.ProviderName;
        return provider is not null
            && provider.Contains(SqliteProviderToken, StringComparison.OrdinalIgnoreCase);
    }
}
