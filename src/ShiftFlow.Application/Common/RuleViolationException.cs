namespace ShiftFlow.Application.Common;

/// <summary>
/// Rechazo de <c>AssignShift</c> por hard rule, con explicación adjunta (SPEC-APP-005 §4).
/// </summary>
public sealed class RuleViolationException : Exception
{
    /// <summary>
    /// Crea el rechazo con código de dominio y texto de explicación.
    /// </summary>
    /// <param name="code">Código HR-* del Rule Engine.</param>
    /// <param name="message">Mensaje corto de dominio.</param>
    /// <param name="title">Título de la explicación.</param>
    /// <param name="body">Cuerpo de la explicación.</param>
    public RuleViolationException(string code, string message, string title, string body)
        : base(message)
    {
        Code = code;
        Title = title;
        Body = body;
        MutatesSchedule = false;
    }

    /// <summary>Código de la hard rule.</summary>
    public string Code { get; }

    /// <summary>Título corto en castellano.</summary>
    public string Title { get; }

    /// <summary>Cuerpo de la explicación.</summary>
    public string Body { get; }

    /// <summary>Siempre <c>false</c>: este error no persiste el cuadrante.</summary>
    public bool MutatesSchedule { get; }
}
