namespace ShiftFlow.Domain.Leaves;

/// <summary>
/// Estado del agregado Leave (SPEC-DOM-007).
/// </summary>
public enum LeaveStatus
{
    /// <summary>
    /// Ausencia vigente; participa en HR-02 y listados activos.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Ausencia cancelada; no bloquea asignaciones.
    /// </summary>
    Cancelled = 1
}
