using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using ShiftFlow.Application.Departments;
using ShiftFlow.Application.Employees;
using ShiftFlow.Application.Leaves;
using ShiftFlow.Application.Organizations;
using ShiftFlow.Application.ShiftAssignments;
using ShiftFlow.Application.ShiftTypes;

namespace ShiftFlow.Web.Api;

/// <summary>
/// Cliente HTTP tipado para maestros, planificación y leaves vía la Api.
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

    /// <summary>Configura el descanso mínimo entre turnos (HR-03) de una organización.</summary>
    public Task<ApiResult<OrganizationDto>> SetOrganizationMinimumRestAsync(
        Guid id,
        int minimumRestMinutes,
        CancellationToken ct = default) =>
        PutAsync<OrganizationDto>(
            $"/api/organizations/{id}/minimum-rest",
            new { minimumRestMinutes },
            ct);

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

    /// <summary>Obtiene la proyección mensual (asignaciones Assigned + leaves Active).</summary>
    public Task<MonthCalendarDto> GetMonthCalendarAsync(
        Guid organizationId,
        int year,
        int month,
        CancellationToken ct = default) =>
        GetAsyncRequired<MonthCalendarDto>(
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

    #region Leaves

    /// <summary>Lista leaves de una organización.</summary>
    public Task<IReadOnlyList<LeaveDto>> ListLeavesAsync(
        Guid organizationId,
        Guid? employeeId = null,
        int? year = null,
        int? month = null,
        bool activeOnly = true,
        CancellationToken ct = default)
    {
        List<string> qs = new List<string> { $"activeOnly={activeOnly}" };
        if (employeeId.HasValue)
        {
            qs.Add($"employeeId={employeeId}");
        }

        if (year.HasValue && month.HasValue)
        {
            qs.Add($"year={year}");
            qs.Add($"month={month}");
        }

        return GetListAsync<LeaveDto>(
            $"/api/organizations/{organizationId}/leaves?{string.Join('&', qs)}",
            ct);
    }

    /// <summary>Registra un leave Active.</summary>
    public Task<ApiResult<LeaveDto>> RegisterLeaveAsync(
        Guid organizationId,
        Guid employeeId,
        DateOnly startOn,
        DateOnly endOn,
        string? kind,
        string? reason,
        CancellationToken ct = default) =>
        PostAsync<LeaveDto>(
            $"/api/organizations/{organizationId}/leaves",
            new { employeeId, startOn, endOn, kind, reason },
            ct);

    /// <summary>Cancela un leave Active.</summary>
    public Task<ApiResult<LeaveDto>> CancelLeaveAsync(Guid leaveId, CancellationToken ct = default) =>
        PostAsync<LeaveDto>($"/api/leaves/{leaveId}/cancel", new { }, ct);

    #endregion

    #region HTTP helpers

    private async Task<T> GetAsyncRequired<T>(string url, CancellationToken ct)
    {
        using HttpResponseMessage? response = await Client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode)
        {
            ApiErrorBody? problem = await TryReadErrorBodyAsync(response, ct);
            throw new HttpRequestException(FormatError(problem, response.StatusCode), null, response.StatusCode);
        }

        T? value = await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
        return value ?? throw new HttpRequestException("Respuesta vacía de la Api.");
    }

    private async Task<IReadOnlyList<T>> GetListAsync<T>(string url, CancellationToken ct)
    {
        using HttpResponseMessage? response = await Client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode)
        {
            ApiErrorBody? problem = await TryReadErrorBodyAsync(response, ct);
            throw new HttpRequestException(FormatError(problem, response.StatusCode), null, response.StatusCode);
        }

        List<T>? list = await response.Content.ReadFromJsonAsync<List<T>>(JsonOptions, ct);
        return list ?? [];
    }

    private async Task<ApiResult<T>> GetAsync<T>(string url, CancellationToken ct)
    {
        using HttpResponseMessage? response = await Client.GetAsync(url, ct);
        return await ToResultAsync<T>(response, ct);
    }

    private async Task<ApiResult<T>> PostAsync<T>(string url, object body, CancellationToken ct)
    {
        using HttpResponseMessage? response = await Client.PostAsJsonAsync(url, body, ct);
        return await ToResultAsync<T>(response, ct);
    }

    private async Task<ApiResult<T>> PutAsync<T>(string url, object body, CancellationToken ct)
    {
        using HttpResponseMessage? response = await Client.PutAsJsonAsync(url, body, ct);
        return await ToResultAsync<T>(response, ct);
    }

    private static async Task<ApiResult<T>> ToResultAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            T? value = await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
            return value is null
                ? ApiResult<T>.Fail("Respuesta vacía de la Api.")
                : ApiResult<T>.Ok(value);
        }

        ApiErrorBody? problem = await TryReadErrorBodyAsync(response, ct);
        string message = FormatError(problem, response.StatusCode);
        if (problem is not null &&
            (!string.IsNullOrWhiteSpace(problem.Title) || !string.IsNullOrWhiteSpace(problem.Body)))
        {
            return ApiResult<T>.Fail(message, problem.Code, problem.Title, problem.Body);
        }

        return ApiResult<T>.Fail(message);
    }

    private static string FormatError(ApiErrorBody? problem, HttpStatusCode statusCode)
    {
        if (problem is not null && !string.IsNullOrWhiteSpace(problem.Error))
        {
            return string.IsNullOrWhiteSpace(problem.Code)
                ? problem.Error
                : $"{problem.Code}: {problem.Error}";
        }

        return $"Error HTTP {(int)statusCode}";
    }

    private static async Task<ApiErrorBody?> TryReadErrorBodyAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<ApiErrorBody>(JsonOptions, ct);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    private sealed record ApiErrorBody(string? Error, string? Code, string? Title, string? Body);
}

/// <summary>
/// Resultado de una llamada a la Api (éxito con valor o mensaje de error).
/// </summary>
/// <typeparam name="T">Tipo del valor.</typeparam>
public sealed class ApiResult<T>
{
    private ApiResult(T? value, string? error, string? errorCode, string? errorTitle, string? errorBody)
    {
        Value = value;
        Error = error;
        ErrorCode = errorCode;
        ErrorTitle = errorTitle;
        ErrorBody = errorBody;
    }

    /// <summary>Valor cuando la llamada tuvo éxito.</summary>
    public T? Value { get; }

    /// <summary>Mensaje de error cuando falló.</summary>
    public string? Error { get; }

    /// <summary>Código de regla o invariante cuando la Api lo envía.</summary>
    public string? ErrorCode { get; }

    /// <summary>Título de explicación (stub PBI-011) cuando el rechazo es HR-*.</summary>
    public string? ErrorTitle { get; }

    /// <summary>Cuerpo de explicación en castellano; no implica mutación del cuadrante.</summary>
    public string? ErrorBody { get; }

    /// <summary>Indica si la llamada tuvo éxito.</summary>
    public bool Succeeded => Error is null && Value is not null;

    /// <summary>Crea un resultado correcto.</summary>
    public static ApiResult<T> Ok(T value) => new(value, null, null, null, null);

    /// <summary>Crea un resultado de error.</summary>
    public static ApiResult<T> Fail(string error) => new(default, error, null, null, null);

    /// <summary>Crea un resultado de error con explicación de regla adjunta.</summary>
    /// <param name="error">Mensaje corto (código + texto de dominio).</param>
    /// <param name="code">Código HR-* o invariante.</param>
    /// <param name="title">Título de la explicación.</param>
    /// <param name="body">Cuerpo de la explicación.</param>
    public static ApiResult<T> Fail(string error, string? code, string? title, string? body) =>
        new(default, error, code, title, body);
}
