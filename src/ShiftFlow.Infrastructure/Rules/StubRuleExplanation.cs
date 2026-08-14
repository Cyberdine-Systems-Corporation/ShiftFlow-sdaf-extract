using ShiftFlow.Application.Rules;

namespace ShiftFlow.Infrastructure.Rules;

/// <summary>
/// Stub determinista de explicación de reglas (ADR-003 / PBI-011). Sin LLM ni red.
/// </summary>
public sealed class StubRuleExplanation : IRuleExplanation
{
    /// <inheritdoc />
    public RuleExplanation Explain(RuleExplanationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        string normalized = Normalize(request.Code);
        return normalized switch
        {
            "HR-01" => Hr01(),
            "HR-02" => Hr02(),
            "HR-03" => Hr03(),
            _ => Unsupported(request.Code)
        };
    }

    private static string Normalize(string? code) =>
        string.IsNullOrWhiteSpace(code) ? string.Empty : code.Trim().ToUpperInvariant();

    private static RuleExplanation Hr01() =>
        new(
            "HR-01",
            "Solape de turnos",
            "La regla HR-01 impide dos turnos Assigned de la misma persona cuyo intervalo se solape. "
            + "Elige otro horario que no cubra el turno ya asignado, o cancela el existente. "
            + "Esta explicación no crea ni modifica turnos.",
            MutatesSchedule: false);

    private static RuleExplanation Hr02() =>
        new(
            "HR-02",
            "Ausencia activa",
            "La regla HR-02 bloquea la asignación porque el empleado tiene un leave (ausencia) Active que cubre el intervalo. "
            + "Cambia las fechas del turno fuera de la ausencia, o cancela el leave si ya no aplica. "
            + "Esta explicación no crea ni modifica turnos ni ausencias.",
            MutatesSchedule: false);

    private static RuleExplanation Hr03() =>
        new(
            "HR-03",
            "Descanso mínimo",
            "La regla HR-03 exige un descanso mínimo (umbral de la organización) entre turnos Assigned de la misma persona. "
            + "Aumenta el hueco respecto al turno vecino o reduce el umbral de descanso de la organización. "
            + "Esta explicación no crea ni modifica turnos.",
            MutatesSchedule: false);

    private static RuleExplanation Unsupported(string? raw)
    {
        string echoed = string.IsNullOrWhiteSpace(raw) ? string.Empty : raw.Trim();
        return new RuleExplanation(
            echoed,
            "Código no soportado",
            "El código indicado no es una hard rule del MVP (HR-01, HR-02 o HR-03). "
            + "El stub no inventa reglas nuevas y no autoriza ninguna asignación.",
            MutatesSchedule: false);
    }
}
