using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShiftFlow.Infrastructure.Persistence;

/// <summary>
/// Factory de diseño para <c>dotnet ef</c> sin levantar Aspire ni el host Api.
/// </summary>
public sealed class ShiftFlowDbContextFactory : IDesignTimeDbContextFactory<ShiftFlowDbContext>
{
    /// <summary>
    /// Crea el contexto con Npgsql. Cadena: <c>ConnectionStrings__shiftflow</c> o el default local del runbook.
    /// </summary>
    /// <param name="args">Argumentos de <c>dotnet ef</c> (no usados).</param>
    /// <returns>Contexto configurado para PostgreSQL.</returns>
    public ShiftFlowDbContext CreateDbContext(string[] args)
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__shiftflow")
            ?? "Host=localhost;Port=5433;Database=shiftflow;Username=shiftflow;Password=shiftflow";

        DbContextOptionsBuilder<ShiftFlowDbContext> optionsBuilder = new DbContextOptionsBuilder<ShiftFlowDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new ShiftFlowDbContext(optionsBuilder.Options);
    }
}
