using ShiftFlow.Domain.ShiftAssignments;

namespace ShiftFlow.Domain.Rules;

/// <summary>
/// Rule Engine v1: evalúa hard rules que bloquean asignaciones (ADR-003 / SPEC-DOM-006).
/// </summary>
public sealed class RuleEngine
{
    /// <summary>
    /// Evalúa las hard rules activas sobre un candidato.
    /// En PBI-005 solo se aplica <c>HR-01</c> (solape); HR-02/HR-03 se activarán con Leave/config.
    /// </summary>
    /// <param name="candidate">Asignación candidata (aún no persistida o no confirmada).</param>
    /// <param name="existingAssigned">Asignaciones <see cref="ShiftAssignmentStatus.Assigned"/> del mismo empleado.</param>
    /// <returns>Lista vacía si no hay violaciones; en caso contrario una o más <see cref="RuleViolation"/>.</returns>
    public IReadOnlyList<RuleViolation> Evaluate(
        ShiftAssignment candidate,
        IReadOnlyList<ShiftAssignment> existingAssigned)
    {
        var violations = new List<RuleViolation>();

        // HR-01: intervalos semiabiertos [StartAt, EndAt); el borde exacto no solapa.
        foreach (var existing in existingAssigned)
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
}
