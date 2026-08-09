namespace ShiftFlow.Web.Auth;

/// <summary>
/// Contenedor de cookies por circuito Blazor Server para reutilizar la sesión Identity de la Api.
/// </summary>
public sealed class CookieContainerHolder
{
    public System.Net.CookieContainer Container { get; } = new();
}
