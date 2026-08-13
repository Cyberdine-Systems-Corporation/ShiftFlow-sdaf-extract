using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class LeaveApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public LeaveApiTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ACC_S2_L01_registrar_leave()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse _) = await SeedOrgAsync(client, "Org Leave Register");

        LeaveResponse? leave = await RegisterLeaveAsync(client, org.Id, emp.Id, new DateOnly(2026, 8, 15), new DateOnly(2026, 8, 15));
        leave.Status.Should().Be("Active");

        List<LeaveResponse>? list = await client.GetFromJsonAsync<List<LeaveResponse>>(
            $"/api/organizations/{org.Id}/leaves",
            JsonOptions);
        list.Should().ContainSingle(x => x.Id == leave.Id && x.EmployeeId == emp.Id);

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Leaves.Should().ContainSingle(x =>
            x.Id == leave.Id
            && x.EmployeeId == emp.Id
            && x.StartOn == leave.StartOn);
    }

    [Fact]
    public async Task ACC_S2_L02_leave_bloquea_asignacion_HR02()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Leave Block");

        await RegisterLeaveAsync(client, org.Id, emp.Id, new DateOnly(2026, 8, 15), new DateOnly(2026, 8, 15));

        DateTimeOffset day = new DateTimeOffset(2026, 8, 15, 0, 0, 0, TimeSpan.Zero);
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(10),
                endAt = day.AddHours(14)
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await response.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("HR-02");
    }

    [Fact]
    public async Task ACC_S2_L03_asignacion_fuera_del_leave_permitida()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Leave Outside");

        await RegisterLeaveAsync(client, org.Id, emp.Id, new DateOnly(2026, 8, 15), new DateOnly(2026, 8, 15));

        DateTimeOffset day = new DateTimeOffset(2026, 8, 16, 10, 0, 0, TimeSpan.Zero);
        ShiftAssignmentResponse? assign = await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day, day.AddHours(4));
        assign.Status.Should().Be("Assigned");
    }

    [Fact]
    public async Task ACC_S2_L04_cancelar_leave_desbloquea()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Leave Cancel");

        LeaveResponse? leave = await RegisterLeaveAsync(
            client,
            org.Id,
            emp.Id,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));

        HttpResponseMessage? cancel = await client.PostAsync($"/api/leaves/{leave.Id}/cancel", null);
        cancel.StatusCode.Should().Be(HttpStatusCode.OK);

        DateTimeOffset day = new DateTimeOffset(2026, 8, 15, 10, 0, 0, TimeSpan.Zero);
        ShiftAssignmentResponse? assign = await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day, day.AddHours(4));
        assign.Status.Should().Be("Assigned");
    }

    [Fact]
    public async Task ACC_S2_L05_rechazo_EndOn_anterior()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse _) = await SeedOrgAsync(client, "Org Leave Invalid Range");

        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/leaves",
            new
            {
                employeeId = emp.Id,
                startOn = "2026-08-16",
                endOn = "2026-08-15",
                kind = (string?)null,
                reason = (string?)null
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await response.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("INV-LEA-03");
    }

    [Fact]
    public async Task ACC_S2_L06_escritura_anonima_rechazada()
    {
        HttpClient? client = _factory.CreateClient();
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{Guid.NewGuid()}/leaves",
            new
            {
                employeeId = Guid.NewGuid(),
                startOn = "2026-08-15",
                endOn = "2026-08-15",
                kind = (string?)null,
                reason = (string?)null
            });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ACC_S2_L07_HR01_y_HR02_distinguibles()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Codes Distinct");

        DateTimeOffset day10 = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day10.AddHours(10), day10.AddHours(14));

        HttpResponseMessage? overlap = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day10.AddHours(12),
                endAt = day10.AddHours(16)
            });
        overlap.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await overlap.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions))!.Code.Should().Be("HR-01");

        await RegisterLeaveAsync(client, org.Id, emp.Id, new DateOnly(2026, 8, 15), new DateOnly(2026, 8, 15));
        DateTimeOffset day15 = new DateTimeOffset(2026, 8, 15, 0, 0, 0, TimeSpan.Zero);
        HttpResponseMessage? underLeave = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day15.AddHours(10),
                endAt = day15.AddHours(14)
            });
        underLeave.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await underLeave.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions))!.Code.Should().Be("HR-02");
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

    private static async Task<LeaveResponse> RegisterLeaveAsync(
        HttpClient client,
        Guid organizationId,
        Guid employeeId,
        DateOnly startOn,
        DateOnly endOn)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/leaves",
            new
            {
                employeeId,
                startOn = startOn.ToString("yyyy-MM-dd"),
                endOn = endOn.ToString("yyyy-MM-dd"),
                kind = "Vacation",
                reason = "Demo"
            });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        LeaveResponse? body = await response.Content.ReadFromJsonAsync<LeaveResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
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
        return (await response.Content.ReadFromJsonAsync<ShiftAssignmentResponse>(JsonOptions))!;
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

    private sealed record LeaveResponse(
        Guid Id,
        Guid OrganizationId,
        Guid EmployeeId,
        DateOnly StartOn,
        DateOnly EndOn,
        string Status,
        string? Kind,
        string? Reason);

    private sealed record MonthCalendarResponse(
        List<CalendarAssignmentItem> Assignments,
        List<CalendarLeaveItem> Leaves);

    private sealed record CalendarAssignmentItem(
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
