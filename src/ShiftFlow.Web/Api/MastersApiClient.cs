using System.Net.Http.Json;
using System.Text.Json;
using ShiftFlow.Application.Departments;
using ShiftFlow.Application.Employees;
using ShiftFlow.Application.Organizations;
using ShiftFlow.Application.ShiftAssignments;
using ShiftFlow.Application.ShiftTypes;

namespace ShiftFlow.Web.Api;

/// <summary>
/// Cliente HTTP tipado para maestros y planificación (calendario / AssignShift) vía la Api.
/// </summary>
public sealed class MastersApiClient(IHttpClientFactory httpClientFactory)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private HttpClient Client => httpClientFactory.CreateClient("api");

    #region Organizations

    /// <summary>Lista organizaciones.</summary>
    public Task<IReadOnlyList<OrganizationDto>> ListOrganizationsAsync(CancellationToken ct = default) =>
        GetListAsync<OrganizationDto>("/api/organizations", ct);

    /// <summary>Crea una organización.</summary>
    public Task<ApiResult<OrganizationDto>> CreateOrganizationAsync(string name, CancellationToken ct = default) =>
        PostAsync<OrganizationDto>("/api/organizations", new { name }, ct);

    /// <summary>Obtiene una organización por id.</summary>
    public Task<ApiResult<OrganizationDto>> GetOrganizationAsync(Guid id, CancellationToken ct = default) =>
        GetAsync<OrganizationDto>($"/api/organizations/{id}", ct);

    /// <summary>Renombra una organización.</summary>
    public Task<ApiResult<OrganizationDto>> RenameOrganizationAsync(Guid id, string name, CancellationToken ct = default) =>
        PutAsync<OrganizationDto>($"/api/organizations/{id}/name", new { name }, ct);

    /// <summary>Activa o desactiva una organización.</summary>
    public Task<ApiResult<OrganizationDto>> SetOrganizationActiveAsync(Guid id, bool isActive, CancellationToken ct = default) =>
        PutAsync<OrganizationDto>($"/api/organizations/{id}/active", new { isActive }, ct);

    #endregion

    #region Departments

    /// <summary>Lista departamentos de una organización.</summary>
    public Task<IReadOnlyList<DepartmentDto>> ListDepartmentsAsync(Guid organizationId, CancellationToken ct = default) =>
        GetListAsync<DepartmentDto>($"/api/organizations/{organizationId}/departments", ct);

    /// <summary>Crea un departamento.</summary>
    public Task<ApiResult<DepartmentDto>> CreateDepartmentAsync(Guid organizationId, string name, CancellationToken ct = default) =>
        PostAsync<DepartmentDto>($"/api/organizations/{organizationId}/departments", new { name }, ct);

    /// <summary>Renombra un departamento.</summary>
    public Task<ApiResult<DepartmentDto>> RenameDepartmentAsync(Guid id, string name, CancellationToken ct = default) =>
        PutAsync<DepartmentDto>($"/api/departments/{id}/name", new { name }, ct);

    /// <summary>Activa o desactiva un departamento.</summary>
    public Task<ApiResult<DepartmentDto>> SetDepartmentActiveAsync(Guid id, bool isActive, CancellationToken ct = default) =>
        PutAsync<DepartmentDto>($"/api/departments/{id}/active", new { isActive }, ct);

    #endregion

    #region Employees

    /// <summary>Lista empleados de una organización.</summary>
    public Task<IReadOnlyList<EmployeeDto>> ListEmployeesAsync(Guid organizationId, CancellationToken ct = default) =>
        GetListAsync<EmployeeDto>($"/api/organizations/{organizationId}/employees", ct);

    /// <summary>Crea un empleado.</summary>
    public Task<ApiResult<EmployeeDto>> CreateEmployeeAsync(
        Guid organizationId,
        Guid departmentId,
        string displayName,
        string? email,
        CancellationToken ct = default) =>
        PostAsync<EmployeeDto>(
            $"/api/organizations/{organizationId}/employees",
            new { departmentId, displayName, email },
            ct);

    /// <summary>Actualiza un empleado.</summary>
    public Task<ApiResult<EmployeeDto>> UpdateEmployeeAsync(
        Guid id,
        Guid departmentId,
        string displayName,
        string? email,
        CancellationToken ct = default) =>
        PutAsync<EmployeeDto>($"/api/employees/{id}", new { departmentId, displayName, email }, ct);

    /// <summary>Activa o desactiva un empleado.</summary>
    public Task<ApiResult<EmployeeDto>> SetEmployeeActiveAsync(Guid id, bool isActive, CancellationToken ct = default) =>
        PutAsync<EmployeeDto>($"/api/employees/{id}/active", new { isActive }, ct);

    #endregion

    #region ShiftTypes

    /// <summary>Lista tipos de turno de una organización.</summary>
    public Task<IReadOnlyList<ShiftTypeDto>> ListShiftTypesAsync(Guid organizationId, CancellationToken ct = default) =>
        GetListAsync<ShiftTypeDto>($"/api/organizations/{organizationId}/shift-types", ct);

    /// <summary>Crea un tipo de turno.</summary>
    public Task<ApiResult<ShiftTypeDto>> CreateShiftTypeAsync(
        Guid organizationId,
        string name,
        string? code,
        TimeOnly? defaultStartTime,
        TimeOnly? defaultEndTime,
        CancellationToken ct = default) =>
        PostAsync<ShiftTypeDto>(
            $"/api/organizations/{organizationId}/shift-types",
            new { name, code, defaultStartTime, defaultEndTime },
            ct);

    /// <summary>Actualiza un tipo de turno.</summary>
    public Task<ApiResult<ShiftTypeDto>> UpdateShiftTypeAsync(
        Guid id,
        string name,
        string? code,
        TimeOnly? defaultStartTime,
        TimeOnly? defaultEndTime,
        CancellationToken ct = default) =>
        PutAsync<ShiftTypeDto>(
            $"/api/shift-types/{id}",
            new { name, code, defaultStartTime, defaultEndTime },
            ct);

    /// <summary>Activa o desactiva un tipo de turno.</summary>
    public Task<ApiResult<ShiftTypeDto>> SetShiftTypeActiveAsync(Guid id, bool isActive, CancellationToken ct = default) =>
        PutAsync<ShiftTypeDto>($"/api/shift-types/{id}/active", new { isActive }, ct);

    #endregion

    #region Calendar & Assignments

    /// <summary>Obtiene las asignaciones Assigned del mes civil.</summary>
    public Task<IReadOnlyList<CalendarAssignmentDto>> GetMonthCalendarAsync(
        Guid organizationId,
        int year,
        int month,
        CancellationToken ct = default) =>
        GetListAsync<CalendarAssignmentDto>(
            $"/api/organizations/{organizationId}/calendar?year={year}&month={month}",
            ct);

    /// <summary>Asigna un turno (invoca Rule Engine en la Api antes de persistir).</summary>
    public Task<ApiResult<ShiftAssignmentDto>> AssignShiftAsync(
        Guid organizationId,
        Guid employeeId,
        Guid shiftTypeId,
        DateTimeOffset startAt,
        DateTimeOffset endAt,
        CancellationToken ct = default) =>
        PostAsync<ShiftAssignmentDto>(
            $"/api/organizations/{organizationId}/assignments",
            new { employeeId, shiftTypeId, startAt, endAt },
            ct);

    /// <summary>Cancela una asignación Assigned.</summary>
    public Task<ApiResult<ShiftAssignmentDto>> CancelShiftAsync(Guid assignmentId, CancellationToken ct = default) =>
        PostAsync<ShiftAssignmentDto>($"/api/assignments/{assignmentId}/cancel", new { }, ct);

    #endregion

    #region HTTP helpers

    private async Task<IReadOnlyList<T>> GetListAsync<T>(string url, CancellationToken ct)
    {
        using var response = await Client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode)
        {
            var message = await ReadErrorAsync(response, ct);
            throw new HttpRequestException(message, null, response.StatusCode);
        }

        var list = await response.Content.ReadFromJsonAsync<List<T>>(JsonOptions, ct);
        return list ?? [];
    }

    private async Task<ApiResult<T>> GetAsync<T>(string url, CancellationToken ct)
    {
        using var response = await Client.GetAsync(url, ct);
        return await ToResultAsync<T>(response, ct);
    }

    private async Task<ApiResult<T>> PostAsync<T>(string url, object body, CancellationToken ct)
    {
        using var response = await Client.PostAsJsonAsync(url, body, ct);
        return await ToResultAsync<T>(response, ct);
    }

    private async Task<ApiResult<T>> PutAsync<T>(string url, object body, CancellationToken ct)
    {
        using var response = await Client.PutAsJsonAsync(url, body, ct);
        return await ToResultAsync<T>(response, ct);
    }

    private static async Task<ApiResult<T>> ToResultAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            var value = await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
            return value is null
                ? ApiResult<T>.Fail("Respuesta vacía de la Api.")
                : ApiResult<T>.Ok(value);
        }

        var message = await ReadErrorAsync(response, ct);
        return ApiResult<T>.Fail(message);
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ApiErrorBody>(JsonOptions, ct);
            if (problem is not null && !string.IsNullOrWhiteSpace(problem.Error))
            {
                return string.IsNullOrWhiteSpace(problem.Code)
                    ? problem.Error
                    : $"{problem.Code}: {problem.Error}";
            }
        }
        catch
        {
            // ignore parse errors
        }

        return $"Error HTTP {(int)response.StatusCode}";
    }

    #endregion

    private sealed record ApiErrorBody(string? Error, string? Code);
}

/// <summary>
/// Resultado de una llamada a la Api (éxito con valor o mensaje de error).
/// </summary>
/// <typeparam name="T">Tipo del valor.</typeparam>
public sealed class ApiResult<T>
{
    private ApiResult(T? value, string? error)
    {
        Value = value;
        Error = error;
    }

    /// <summary>Valor cuando la llamada tuvo éxito.</summary>
    public T? Value { get; }

    /// <summary>Mensaje de error cuando falló.</summary>
    public string? Error { get; }

    /// <summary>Indica si la llamada tuvo éxito.</summary>
    public bool Succeeded => Error is null && Value is not null;

    /// <summary>Crea un resultado correcto.</summary>
    public static ApiResult<T> Ok(T value) => new(value, null);

    /// <summary>Crea un resultado de error.</summary>
    public static ApiResult<T> Fail(string error) => new(default, error);
}
