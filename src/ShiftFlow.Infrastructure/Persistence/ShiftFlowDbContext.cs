using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftAssignments;
using ShiftFlow.Domain.ShiftTypes;
using ShiftFlow.Infrastructure.Identity;

namespace ShiftFlow.Infrastructure.Persistence;

/// <summary>
/// Contexto EF Core de ShiftFlow: agregados de dominio e Identity.
/// </summary>
public sealed class ShiftFlowDbContext(DbContextOptions<ShiftFlowDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IUnitOfWork
{
    #region Sets

    /// <summary>
    /// Conjunto de organizaciones.
    /// </summary>
    public DbSet<Organization> Organizations => Set<Organization>();

    /// <summary>
    /// Conjunto de departamentos.
    /// </summary>
    public DbSet<Department> Departments => Set<Department>();

    /// <summary>
    /// Conjunto de empleados.
    /// </summary>
    public DbSet<Employee> Employees => Set<Employee>();

    /// <summary>
    /// Conjunto de tipos de turno.
    /// </summary>
    public DbSet<ShiftType> ShiftTypes => Set<ShiftType>();

    /// <summary>
    /// Conjunto de asignaciones de turno.
    /// </summary>
    public DbSet<ShiftAssignment> ShiftAssignments => Set<ShiftAssignment>();

    #endregion

    #region Configuration

    /// <summary>
    /// Aplica las configuraciones Fluent API del ensamblado Infrastructure.
    /// </summary>
    /// <param name="builder">Constructor del modelo EF Core.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ShiftFlowDbContext).Assembly);
    }

    #endregion
}
