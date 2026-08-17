namespace ShiftFlow.Web;

/// <summary>
/// Organización activa del workspace de demo, compartida entre shell, calendario y ausencias.
/// </summary>
public sealed class WorkspaceContext
{
    /// <summary>
    /// Identificador de la organización seleccionada, o <see langword="null"/> si no hay ninguna.
    /// </summary>
    public Guid? SelectedOrganizationId { get; private set; }

    /// <summary>
    /// Se dispara al cambiar la organización activa.
    /// </summary>
    public event Action? Changed;

    /// <summary>
    /// Establece la organización activa. No notifica si el valor no cambia.
    /// </summary>
    /// <param name="organizationId">Identificador, o <see langword="null"/> para limpiar.</param>
    public void SetOrganization(Guid? organizationId)
    {
        if (SelectedOrganizationId == organizationId)
        {
            return;
        }

        SelectedOrganizationId = organizationId;
        Changed?.Invoke();
    }
}
