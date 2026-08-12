namespace ShiftFlow.Domain.ShiftAssignments;

/// <summary>
/// Estado del ciclo de vida de una asignación de turno (MVP).
/// </summary>
public enum ShiftAssignmentStatus
{
    /// <summary>
    /// Turno vigente; participa en calendario y en evaluación de solapes.
    /// </summary>
    Assigned = 0,

    /// <summary>
    /// Turno cancelado; no aparece en calendario ni en solapes.
    /// </summary>
    Cancelled = 1
}
