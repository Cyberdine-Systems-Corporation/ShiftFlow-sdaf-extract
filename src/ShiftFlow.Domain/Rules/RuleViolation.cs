namespace ShiftFlow.Domain.Rules;

/// <summary>
/// Violación de una hard rule del Rule Engine v1.
/// </summary>
/// <param name="Code">Código estable de regla (p. ej. <c>HR-01</c>).</param>
/// <param name="Message">Mensaje observable para API/UI.</param>
public sealed record RuleViolation(string Code, string Message);
