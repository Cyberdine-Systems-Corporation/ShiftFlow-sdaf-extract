using ShiftFlow.Domain.Leaves;
using ShiftFlow.Domain.ShiftAssignments;

namespace ShiftFlow.Domain.Rules;

/// <summary>
/// Rule Engine v1: evalúa hard rules que bloquean asignaciones (ADR-003 / SPEC-DOM-006).
/// </summary>
public sealed class RuleEngine
{
    /// <summary>
    /// Evalúa las hard rules activas sobre un candidato (<c>HR-01</c> solape, <c>HR-02</c> leave, <c>HR-03</c> descanso).
    /// </summary>
    /// <param name="candidate">Asignación candidata (aún no persistida o no confirmada).</param>
    /// <param name="existingAssigned">Asignaciones <see cref="ShiftAssignmentStatus.Assigned"/> del mismo empleado.</param>
    /// <param name="activeLeaves">Leaves <see cref="LeaveStatus.Active"/> del mismo empleado (vacío si no hay).</param>
    /// <param name="minimumRest">Umbral de descanso mínimo (HR-03); <c>null</c> o cero no aplica la regla.</param>
    /// <returns>Lista vacía si no hay violaciones; en caso contrario una o más <see cref="RuleViolation"/>.</returns>
    public IReadOnlyList<RuleViolation> Evaluate(
        ShiftAssignment candidate,
        IReadOnlyList<ShiftAssignment> existingAssigned,
        IReadOnlyList<Leave>? activeLeaves = null,
        TimeSpan? minimumRest = null)
    {
        List<RuleViolation> violations = new List<RuleViolation>();

        // HR-01: intervalos semiabiertos [StartAt, EndAt); el borde exacto no solapa.
        foreach (ShiftAssignment existing in existingAssigned)
        {
            if (existing.EmployeeId != candidate.EmployeeId)
            {
                continue;
            }

            if (existing.Status != ShiftAssignmentStatus.Assigned)
            {
                continue;
            }

            if (Overlaps(candidate.StartAt, candidate.EndAt, existing.StartAt, existing.EndAt))
            {
                violations.Add(new RuleViolation(
                    "HR-01",
                    "Violación de solape: la misma persona ya tiene un turno Assigned en un intervalo solapado."));
                break;
            }
        }

        // HR-02: Leave Active cuya cobertura intersecta el intervalo candidato.
        if (activeLeaves is { Count: > 0 })
        {
            foreach (Leave leave in activeLeaves)
            {
                if (leave.EmployeeId != candidate.EmployeeId)
                {
                    continue;
                }

                if (leave.Status != LeaveStatus.Active)
                {
                    continue;
                }

                if (leave.CoversInterval(candidate.StartAt, candidate.EndAt))
                {
                    violations.Add(new RuleViolation(
                        "HR-02",
                        "Violación por ausencia: el empleado tiene un Leave activo que cubre el intervalo del turno."));
                    break;
                }
            }
        }

        // HR-03: gap entre turnos Assigned no solapados < umbral de Organization.
        if (minimumRest is { } rest && rest > TimeSpan.Zero)
        {
            foreach (ShiftAssignment existing in existingAssigned)
            {
                if (existing.EmployeeId != candidate.EmployeeId
                    || existing.Status != ShiftAssignmentStatus.Assigned)
                {
                    continue;
                }

                if (Overlaps(candidate.StartAt, candidate.EndAt, existing.StartAt, existing.EndAt))
                {
                    continue;
                }

                TimeSpan gap = GapBetween(candidate.StartAt, candidate.EndAt, existing.StartAt, existing.EndAt);
                if (gap < rest)
                {
                    violations.Add(new RuleViolation(
                        "HR-03",
                        "Violación de descanso mínimo: el intervalo respecto a otro turno Assigned es inferior al umbral de la organización."));
                    break;
                }
            }
        }

        return violations;
    }

    /// <summary>
    /// Determina si dos intervalos semiabiertos se solapan.
    /// </summary>
    internal static bool Overlaps(
        DateTimeOffset startA,
        DateTimeOffset endA,
        DateTimeOffset startB,
        DateTimeOffset endB) =>
        startA < endB && startB < endA;

    /// <summary>
    /// Tiempo entre el fin de un intervalo y el inicio del otro (sin solape).
    /// </summary>
    internal static TimeSpan GapBetween(
        DateTimeOffset startA,
        DateTimeOffset endA,
        DateTimeOffset startB,
        DateTimeOffset endB)
    {
        if (endA <= startB)
        {
            return startB - endA;
        }

        if (endB <= startA)
        {
            return startA - endB;
        }

        return TimeSpan.Zero;
    }
}
