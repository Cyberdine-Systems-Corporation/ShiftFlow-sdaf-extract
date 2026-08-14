using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Leaves;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftAssignments;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.Infrastructure.Persistence;

/// <summary>
/// Catálogo de demo (PBI-010): dos organizaciones de vitrina con casuísticas del MVP. No es migración ni fixture de test.
/// </summary>
public static class DemoCatalogSeed
{
    /// <summary>
    /// Organización ancla de operación (HR-01/HR-02, umbral 0).
    /// </summary>
    public const string OperationOrganizationName = "Demo — Operación";

    /// <summary>
    /// Organización ancla de descanso mínimo (HR-03).
    /// </summary>
    public const string RestOrganizationName = "Demo — Descanso";

    /// <summary>
    /// Clave de configuración para habilitar el catálogo.
    /// </summary>
    public const string SeedCatalogConfigurationKey = "Demo:SeedCatalog";

    /// <summary>
    /// Si el catálogo está habilitado y no existe la org ancla, siembra datos del mes en curso.
    /// </summary>
    /// <param name="db">Contexto ya resuelto en un scope.</param>
    /// <param name="configuration">Flag <c>Demo:SeedCatalog</c>.</param>
    /// <param name="logger">Registro de omisión o siembra.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public static async Task EnsureAsync(
        ShiftFlowDbContext db,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        if (DatabaseInitializer.IsSqlite(db))
        {
            logger.LogInformation("Catálogo de demo omitido (SQLite / tests; H16).");
            return;
        }

        if (!configuration.GetValue(SeedCatalogConfigurationKey, false))
        {
            logger.LogInformation("Catálogo de demo omitido ({Key}=false).", SeedCatalogConfigurationKey);
            return;
        }

        bool alreadySeeded = await db.Organizations
            .AnyAsync(organization => organization.Name == OperationOrganizationName, cancellationToken);
        if (alreadySeeded)
        {
            logger.LogInformation("Catálogo de demo ya presente ({Org}); no se duplica.", OperationOrganizationName);
            return;
        }

        DateOnly todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        SeedOperationCatalog(db, todayUtc);
        SeedRestCatalog(db, todayUtc);
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Catálogo de demo sembrado: {Operation} y {Rest}.",
            OperationOrganizationName,
            RestOrganizationName);
    }

    #region Catalog

    private static void SeedOperationCatalog(ShiftFlowDbContext db, DateOnly today)
    {
        Organization organization = Organization.Create(OperationOrganizationName);
        db.Organizations.Add(organization);

        Department department = Department.Create(organization.Id, "Urgencias", organization.IsActive);
        db.Departments.Add(department);

        ShiftType morning = ShiftType.Create(
            organization.Id,
            organization.IsActive,
            "Mañana",
            "MAN",
            new TimeOnly(8, 0),
            new TimeOnly(14, 0));
        ShiftType afternoon = ShiftType.Create(
            organization.Id,
            organization.IsActive,
            "Tarde",
            "TAR",
            new TimeOnly(14, 0),
            new TimeOnly(18, 0));
        ShiftType night = ShiftType.Create(
            organization.Id,
            organization.IsActive,
            "Noche (inactivo)",
            "NOC",
            new TimeOnly(18, 0),
            new TimeOnly(22, 0));
        night.SetActive(false);
        db.ShiftTypes.AddRange(morning, afternoon, night);

        Employee ana = CreateEmployee(organization, department, "Ana Pérez", "ana.perez@demo.shiftflow.local");
        Employee bruno = CreateEmployee(organization, department, "Bruno Ruiz", "bruno.ruiz@demo.shiftflow.local");
        Employee carla = CreateEmployee(organization, department, "Carla Díaz", "carla.diaz@demo.shiftflow.local");
        Employee elena = CreateEmployee(organization, department, "Elena Soto", "elena.soto@demo.shiftflow.local");
        elena.SetActive(false);
        Employee fran = CreateEmployee(organization, department, "Fran Mora", "fran.mora@demo.shiftflow.local");
        db.Employees.AddRange(ana, bruno, carla, elena, fran);

        db.ShiftAssignments.Add(Assign(organization, ana, morning, AtUtc(today, 8, 0), AtUtc(today, 14, 0)));
        db.ShiftAssignments.Add(Assign(organization, bruno, morning, AtUtc(today, 10, 0), AtUtc(today, 14, 0)));
        db.ShiftAssignments.Add(Assign(organization, bruno, afternoon, AtUtc(today, 14, 0), AtUtc(today, 18, 0)));

        ShiftAssignment cancelled = Assign(organization, fran, morning, AtUtc(today, 9, 0), AtUtc(today, 11, 0));
        cancelled.Cancel();
        db.ShiftAssignments.Add(cancelled);

        Leave activeLeave = Leave.Create(
            organization.Id,
            carla.Id,
            carla.OrganizationId,
            carla.IsActive,
            today,
            today.AddDays(2),
            "Vacation",
            "Vacaciones (bloquea HR-02)");
        Leave cancelledLeave = Leave.Create(
            organization.Id,
            carla.Id,
            carla.OrganizationId,
            carla.IsActive,
            today.AddDays(10),
            today.AddDays(12),
            "Other",
            "Ausencia cancelada (no bloquea)");
        cancelledLeave.Cancel();
        db.Leaves.AddRange(activeLeave, cancelledLeave);
    }

    private static void SeedRestCatalog(ShiftFlowDbContext db, DateOnly today)
    {
        Organization organization = Organization.Create(RestOrganizationName);
        organization.SetMinimumRestMinutes(660);
        db.Organizations.Add(organization);

        Department department = Department.Create(organization.Id, "Planta", organization.IsActive);
        db.Departments.Add(department);

        ShiftType fullDay = ShiftType.Create(
            organization.Id,
            organization.IsActive,
            "Jornada",
            "JOR",
            new TimeOnly(8, 0),
            new TimeOnly(20, 0));
        db.ShiftTypes.Add(fullDay);

        Employee diego = CreateEmployee(organization, department, "Diego López", "diego.lopez@demo.shiftflow.local");
        db.Employees.Add(diego);
        db.ShiftAssignments.Add(Assign(organization, diego, fullDay, AtUtc(today, 8, 0), AtUtc(today, 20, 0)));
    }

    #endregion

    #region Helpers

    private static Employee CreateEmployee(
        Organization organization,
        Department department,
        string displayName,
        string email)
    {
        return Employee.Create(
            organization.Id,
            department.Id,
            department.OrganizationId,
            department.IsActive,
            displayName,
            email);
    }

    private static ShiftAssignment Assign(
        Organization organization,
        Employee employee,
        ShiftType shiftType,
        DateTimeOffset startAt,
        DateTimeOffset endAt)
    {
        return ShiftAssignment.Create(
            organization.Id,
            employee.Id,
            employee.OrganizationId,
            employee.IsActive,
            shiftType.Id,
            shiftType.OrganizationId,
            shiftType.IsActive,
            startAt,
            endAt);
    }

    /// <summary>
    /// Instante UTC (offset 0). Npgsql solo acepta eso en <c>timestamp with time zone</c>.
    /// </summary>
    private static DateTimeOffset AtUtc(DateOnly day, int hour, int minute) =>
        new DateTimeOffset(day.ToDateTime(new TimeOnly(hour, minute)), TimeSpan.Zero);

    #endregion
}
