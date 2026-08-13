using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class CalendarAssignApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public CalendarAssignApiTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ACC_S2_01_abrir_calendario_mensual_vacio()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        OrganizationResponse? org = await CreateOrganizationAsync(client, "Org Calendario");

        HttpResponseMessage? response = await client.GetAsync($"/api/organizations/{org.Id}/calendar?year=2026&month=8");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        MonthCalendarResponse? calendar = await response.Content.ReadFromJsonAsync<MonthCalendarResponse>(JsonOptions);
        calendar.Should().NotBeNull();
        calendar!.Assignments.Should().BeEmpty();
        calendar.Leaves.Should().BeEmpty();
    }

    [Fact]
    public async Task ACC_S2_02_asignacion_valida_visible_en_calendario()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Assign OK");

        DateTimeOffset start = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        DateTimeOffset end = start.AddHours(4);
        ShiftAssignmentResponse? assign = await AssignAsync(client, org.Id, emp.Id, shiftType.Id, start, end);

        assign.Status.Should().Be("Assigned");

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);

        calendar!.Assignments.Should().ContainSingle(x =>
            x.Id == assign.Id
            && x.EmployeeId == emp.Id
            && x.ShiftTypeId == shiftType.Id);
    }

    [Fact]
    public async Task ACC_S2_03_rechazo_por_solape_HR01()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Solape");

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(10), day.AddHours(14));

        HttpResponseMessage? overlap = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(12),
                endAt = day.AddHours(16)
            });

        overlap.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await overlap.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("HR-01");

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().ContainSingle();
    }

    [Fact]
    public async Task ACC_S2_04_turnos_adyacentes_permitidos()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Adyacentes");

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(10), day.AddHours(14));
        ShiftAssignmentResponse? second = await AssignAsync(
            client,
            org.Id,
            emp.Id,
            shiftType.Id,
            day.AddHours(14),
            day.AddHours(18));

        second.Status.Should().Be("Assigned");

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().HaveCount(2);
    }

    [Fact]
    public async Task ACC_S2_05_rechazo_shift_type_inactivo()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Tipo Inactivo");

        HttpResponseMessage? deactivate = await client.PutAsJsonAsync(
            $"/api/shift-types/{shiftType.Id}/active",
            new { isActive = false });
        deactivate.EnsureSuccessStatusCode();

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day,
                endAt = day.AddHours(4)
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await response.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("INV-ASN-03");
    }

    [Fact]
    public async Task ACC_S2_06_cancelar_asignacion()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Cancel");

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        ShiftAssignmentResponse? assign = await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day, day.AddHours(4));

        HttpResponseMessage? cancel = await client.PostAsync($"/api/assignments/{assign.Id}/cancel", null);
        cancel.StatusCode.Should().Be(HttpStatusCode.OK);

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().BeEmpty();
    }

    [Fact]
    public async Task ACC_S2_07_escritura_anonima_rechazada()
    {
        HttpClient? client = _factory.CreateClient();
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{Guid.NewGuid()}/assignments",
            new
            {
                employeeId = Guid.NewGuid(),
                shiftTypeId = Guid.NewGuid(),
                startAt = DateTimeOffset.UtcNow,
                endAt = DateTimeOffset.UtcNow.AddHours(1)
            });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        HttpClient? client = _factory.CreateClient(new() { HandleCookies = true });
        HttpResponseMessage? login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = DemoCredentials.DefaultDevelopmentPassword
        });
        login.EnsureSuccessStatusCode();
        return client;
    }

    [Fact]
    public async Task ACC_S2_R01_rechazo_por_descanso_minimo_HR03()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Rest corto");

        HttpResponseMessage? rest = await client.PutAsJsonAsync(
            $"/api/organizations/{org.Id}/minimum-rest",
            new { minimumRestMinutes = 660 });
        rest.EnsureSuccessStatusCode();

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(8), day.AddHours(16));

        HttpResponseMessage? tooSoon = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(16),
                endAt = day.AddHours(20)
            });

        tooSoon.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await tooSoon.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("HR-03");

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().ContainSingle();
    }

    [Fact]
    public async Task ACC_S2_R02_permite_gap_igual_al_umbral()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Rest OK");

        HttpResponseMessage? rest = await client.PutAsJsonAsync(
            $"/api/organizations/{org.Id}/minimum-rest",
            new { minimumRestMinutes = 660 });
        rest.EnsureSuccessStatusCode();

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(8), day.AddHours(16));

        DateTimeOffset nextStart = day.AddHours(16).AddMinutes(660);
        ShiftAssignmentResponse? second = await AssignAsync(
            client,
            org.Id,
            emp.Id,
            shiftType.Id,
            nextStart,
            nextStart.AddHours(4));

        second.Status.Should().Be("Assigned");
    }

    private static async Task<(OrganizationResponse Org, EmployeeResponse Emp, ShiftTypeResponse ShiftType)> SeedOrgAsync(
        HttpClient client,
        string orgName)
    {
        OrganizationResponse? org = await CreateOrganizationAsync(client, orgName);
        DepartmentResponse? dept = await CreateDepartmentAsync(client, org.Id, "Dept");
        EmployeeResponse? emp = await CreateEmployeeAsync(client, org.Id, dept.Id, "Ana", null);
        ShiftTypeResponse? shiftType = await CreateShiftTypeAsync(client, org.Id, "Mañana", "MAN");
        return (org, emp, shiftType);
    }

    private static async Task<ShiftAssignmentResponse> AssignAsync(
        HttpClient client,
        Guid organizationId,
        Guid employeeId,
        Guid shiftTypeId,
        DateTimeOffset startAt,
        DateTimeOffset endAt)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/assignments",
            new { employeeId, shiftTypeId, startAt, endAt });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        ShiftAssignmentResponse? body = await response.Content.ReadFromJsonAsync<ShiftAssignmentResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
    }

    private static async Task<OrganizationResponse> CreateOrganizationAsync(HttpClient client, string name)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync("/api/organizations", new { name });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<OrganizationResponse>(JsonOptions))!;
    }

    private static async Task<DepartmentResponse> CreateDepartmentAsync(
        HttpClient client,
        Guid organizationId,
        string name)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/departments",
            new { name });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<DepartmentResponse>(JsonOptions))!;
    }

    private static async Task<EmployeeResponse> CreateEmployeeAsync(
        HttpClient client,
        Guid organizationId,
        Guid departmentId,
        string displayName,
        string? email)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/employees",
            new { departmentId, displayName, email });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<EmployeeResponse>(JsonOptions))!;
    }

    private static async Task<ShiftTypeResponse> CreateShiftTypeAsync(
        HttpClient client,
        Guid organizationId,
        string name,
        string? code)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/shift-types",
            new { name, code, defaultStartTime = (string?)null, defaultEndTime = (string?)null });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ShiftTypeResponse>(JsonOptions))!;
    }

    private sealed record OrganizationResponse(Guid Id, string Name, bool IsActive, int MinimumRestMinutes);

    private sealed record DepartmentResponse(Guid Id, Guid OrganizationId, string Name, bool IsActive);

    private sealed record EmployeeResponse(
        Guid Id,
        Guid OrganizationId,
        Guid DepartmentId,
        string DisplayName,
        string? Email,
        bool IsActive);

    private sealed record ShiftTypeResponse(
        Guid Id,
        Guid OrganizationId,
        string Name,
        string? Code,
        TimeOnly? DefaultStartTime,
        TimeOnly? DefaultEndTime,
        bool IsActive);

    private sealed record ShiftAssignmentResponse(
        Guid Id,
        Guid OrganizationId,
        Guid EmployeeId,
        Guid ShiftTypeId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        string Status);

    private sealed record MonthCalendarResponse(
        List<CalendarItem> Assignments,
        List<CalendarLeaveItem> Leaves);

    private sealed record CalendarItem(
        Guid Id,
        Guid EmployeeId,
        string EmployeeDisplayName,
        Guid ShiftTypeId,
        string ShiftTypeName,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt);

    private sealed record CalendarLeaveItem(
        Guid Id,
        Guid EmployeeId,
        string EmployeeDisplayName,
        DateOnly StartOn,
        DateOnly EndOn,
        string? Kind);

    private sealed record ErrorBody(string Error, string Code);
}
