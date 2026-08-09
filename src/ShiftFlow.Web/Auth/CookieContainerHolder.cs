namespace ShiftFlow.Web.Auth;

/// <summary>
/// Contenedor de cookies por circuito Blazor Server para reutilizar la sesión Identity de la Api.
/// </summary>
public sealed class CookieContainerHolder
{
    /// <summary>
    /// Almacén de cookies compartido por el <see cref="System.Net.Http.HttpClient"/> nombrado <c>api</c>.
    /// </summary>
    public System.Net.CookieContainer Container { get; } = new();
}
