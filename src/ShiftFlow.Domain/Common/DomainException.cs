namespace ShiftFlow.Domain.Common;

/// <summary>
/// Excepción de dominio con código de invariante estable (p. ej. INV-ORG-01).
/// </summary>
public sealed class DomainException : Exception
{
    /// <summary>
    /// Crea una excepción de dominio con código y mensaje.
    /// </summary>
    /// <param name="code">Código de invariante o regla de negocio.</param>
    /// <param name="message">Mensaje legible para diagnóstico o mapeo a API.</param>
    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }

    /// <summary>
    /// Código de la regla o invariante incumplida.
    /// </summary>
    public string Code { get; }
}
