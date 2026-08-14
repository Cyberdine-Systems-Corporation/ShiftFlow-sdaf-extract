using MediatR;

namespace ShiftFlow.Application.Rules;

/// <summary>
/// Consulta de explicación de una hard rule (PBI-011). Solo lectura.
/// </summary>
/// <param name="Code">Código de regla (obligatorio).</param>
/// <param name="OrganizationId">Organización opcional.</param>
/// <param name="EmployeeId">Empleado opcional.</param>
/// <param name="StartAt">Inicio opcional del intervalo.</param>
/// <param name="EndAt">Fin opcional del intervalo.</param>
public sealed record ExplainRuleQuery(
    string Code,
    Guid? OrganizationId,
    Guid? EmployeeId,
    DateTimeOffset? StartAt,
    DateTimeOffset? EndAt) : IRequest<RuleExplanation>;

/// <summary>
/// Handler de <see cref="ExplainRuleQuery"/>: delega en el puerto y no toca repositorios de escritura.
/// </summary>
public sealed class ExplainRuleHandler(IRuleExplanation explanations)
    : IRequestHandler<ExplainRuleQuery, RuleExplanation>
{
    /// <summary>
    /// Devuelve la explicación del stub para el código pedido.
    /// </summary>
    /// <param name="request">Consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Explicación; <c>MutatesSchedule</c> falso.</returns>
    /// <exception cref="ArgumentException">Si <see cref="ExplainRuleQuery.Code"/> está vacío.</exception>
    public Task<RuleExplanation> Handle(ExplainRuleQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            throw new ArgumentException("El código de regla es obligatorio.", nameof(request));
        }

        RuleExplanation explanation = explanations.Explain(
            new RuleExplanationRequest(
                request.Code,
                request.OrganizationId,
                request.EmployeeId,
                request.StartAt,
                request.EndAt));
        return Task.FromResult(explanation);
    }
}
