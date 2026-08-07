using Microsoft.EntityFrameworkCore;

namespace ShiftFlow.Infrastructure.Persistence;

/// <summary>
/// DbContext vacío del skeleton; las entidades de dominio se añaden en Sprint 1+.
/// </summary>
public sealed class ShiftFlowDbContext(DbContextOptions<ShiftFlowDbContext> options) : DbContext(options);
