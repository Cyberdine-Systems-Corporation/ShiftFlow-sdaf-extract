namespace ShiftFlow.Application.Common;

/// <summary>
/// Excepción de aplicación cuando no existe el recurso solicitado.
/// </summary>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Crea una excepción de recurso no encontrado.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del recurso ausente.</param>
    public NotFoundException(string message) : base(message)
    {
    }
}
