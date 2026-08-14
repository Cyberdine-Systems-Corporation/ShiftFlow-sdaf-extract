namespace ShiftFlow.Application.Rules;

/// <summary>
/// Puerto de explicación de hard rules. No persiste ni evalúa el Rule Engine (SPEC-APP-005).
/// </summary>
public interface IRuleExplanation
{
    /// <summary>
    /// Devuelve una explicación determinista para el código indicado.
    /// </summary>
    /// <param name="request">Código de regla y contexto opcional (solo redacción).</param>
    /// <returns>Título y cuerpo en castellano; <c>MutatesSchedule</c> siempre falso.</returns>
    RuleExplanation Explain(RuleExplanationRequest request);
}

/// <summary>
/// Entrada del puerto de explicación.
/// </summary>
/// <param name="Code">Código estable (p. ej. <c>HR-01</c>).</param>
/// <param name="OrganizationId">Organización opcional (redacción).</param>
/// <param name="EmployeeId">Empleado opcional (redacción; no se escribe).</param>
/// <param name="StartAt">Inicio opcional del intervalo candidato.</param>
/// <param name="EndAt">Fin opcional del intervalo candidato.</param>
public sealed record RuleExplanationRequest(
    string Code,
    Guid? OrganizationId = null,
    Guid? EmployeeId = null,
    DateTimeOffset? StartAt = null,
    DateTimeOffset? EndAt = null);

/// <summary>
/// Explicación de una hard rule para el planificador.
/// </summary>
/// <param name="Code">Código reconocido o eco del pedido.</param>
/// <param name="Title">Título corto en castellano.</param>
/// <param name="Body">Párrafo que identifica la regla y una pista de qué cambiar, sin proponer un turno persistible.</param>
/// <param name="MutatesSchedule">Siempre <c>false</c> en el MVP.</param>
public sealed record RuleExplanation(
    string Code,
    string Title,
    string Body,
    bool MutatesSchedule);
