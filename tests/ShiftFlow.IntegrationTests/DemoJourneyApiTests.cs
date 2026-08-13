using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

/// <summary>
/// Journey de demo SPEC-PRD-002 (AC-01…AC-05) en un solo flujo API.
/// UI E2E Blazor queda fuera (H16: Playwright opcional).
/// </summary>
[Collection(nameof(ApiCollection))]
public class DemoJourneyApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public DemoJourneyApiTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AC05_api_status_sin_despliegue_cloud()
    {
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("/api/status");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        StatusResponse? payload = await response.Content.ReadFromJsonAsync<StatusResponse>(JsonOptions);
        payload.Should().NotBeNull();
        payload!.Status.Should().Be("ok");
        payload.Service.Should().Be("ShiftFlow.Api");
    }

    [Fact]
    public async Task AC_PRD002_flujo_demo_maestros_asignar_rechazar_leave_y_descanso()
    {
        HttpClient client = await CreateAuthenticatedClientAsync();

        OrganizationResponse org = await PostCreatedAsync<OrganizationResponse>(
            client,
            "/api/organizations",
            new { name = "Org Journey Demo" });
        DepartmentResponse dept = await PostCreatedAsync<DepartmentResponse>(
            client,
            $"/api/organizations/{org.Id}/departments",
            new { name = "Urgencias" });
        EmployeeResponse emp = await PostCreatedAsync<EmployeeResponse>(
            client,
            $"/api/organizations/{org.Id}/employees",
            new { departmentId = dept.Id, displayName = "Ana Pérez", email = "ana@journey.local" });
        ShiftTypeResponse shiftType = await PostCreatedAsync<ShiftTypeResponse>(
            client,
            $"/api/organizations/{org.Id}/shift-types",
            new { name = "Mañana", code = "MAN", defaultStartTime = (string?)null, defaultEndTime = (string?)null });

        List<OrganizationResponse>? orgs = await client.GetFromJsonAsync<List<OrganizationResponse>>(
            "/api/organizations",
            JsonOptions);
        orgs.Should().Contain(o => o.Id == org.Id);

        List<EmployeeResponse>? emps = await client.GetFromJsonAsync<List<EmployeeResponse>>(
            $"/api/organizations/{org.Id}/employees",
            JsonOptions);
        emps.Should().Contain(e => e.Id == emp.Id);

        List<ShiftTypeResponse>? types = await client.GetFromJsonAsync<List<ShiftTypeResponse>>(
            $"/api/organizations/{org.Id}/shift-types",
            JsonOptions);
        types.Should().Contain(s => s.Id == shiftType.Id);

        MonthCalendarResponse? emptyCalendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        emptyCalendar.Should().NotBeNull();
        emptyCalendar!.Assignments.Should().BeEmpty();

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        ShiftAssignmentResponse valid = await AssignAsync(
            client,
            org.Id,
            emp.Id,
            shiftType.Id,
            day.AddHours(8),
            day.AddHours(12));
        valid.Status.Should().Be("Assigned");

        MonthCalendarResponse? afterAssign = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        afterAssign!.Assignments.Should().ContainSingle(x => x.Id == valid.Id);

        HttpResponseMessage overlap = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(10),
                endAt = day.AddHours(14)
            });
        overlap.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? overlapBody = await overlap.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        overlapBody!.Code.Should().Be("HR-01");

        HttpResponseMessage rest = await client.PutAsJsonAsync(
            $"/api/organizations/{org.Id}/minimum-rest",
            new { minimumRestMinutes = 660 });
        rest.EnsureSuccessStatusCode();

        HttpResponseMessage tooSoon = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(12),
                endAt = day.AddHours(16)
            });
        tooSoon.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? restBody = await tooSoon.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        restBody!.Code.Should().Be("HR-03");

        MonthCalendarResponse? afterRules = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        afterRules!.Assignments.Should().ContainSingle(x => x.Id == valid.Id);

        HttpResponseMessage leaveResponse = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/leaves",
            new
            {
                employeeId = emp.Id,
                startOn = "2026-08-15",
                endOn = "2026-08-15",
                kind = "Vacation",
                reason = "Demo"
            });
        leaveResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        DateTimeOffset leaveDay = new DateTimeOffset(2026, 8, 15, 0, 0, 0, TimeSpan.Zero);
        HttpResponseMessage underLeave = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = leaveDay.AddHours(10),
                endAt = leaveDay.AddHours(14)
            });
        underLeave.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? leaveBody = await underLeave.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        leaveBody!.Code.Should().Be("HR-02");
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        HttpClient client = _factory.CreateClient(new() { HandleCookies = true });
        HttpResponseMessage login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            userName = DemoCredentials.UserName,
            password = DemoCredentials.DefaultDevelopmentPassword
        });
        login.EnsureSuccessStatusCode();
        return client;
    }

    private static async Task<T> PostCreatedAsync<T>(HttpClient client, string url, object body)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(url, body);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        T? dto = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        dto.Should().NotBeNull();
        return dto!;
    }

    private static async Task<ShiftAssignmentResponse> AssignAsync(
        HttpClient client,
        Guid organizationId,
        Guid employeeId,
        Guid shiftTypeId,
        DateTimeOffset startAt,
        DateTimeOffset endAt)
    {
        return await PostCreatedAsync<ShiftAssignmentResponse>(
            client,
            $"/api/organizations/{organizationId}/assignments",
            new { employeeId, shiftTypeId, startAt, endAt });
    }

    private sealed record StatusResponse(string Service, string Status, string Database);

    private sealed record OrganizationResponse(Guid Id, string Name, bool IsActive, int MinimumRestMinutes);

    private sealed record DepartmentResponse(Guid Id, Guid OrganizationId, string Name, bool IsActive);

    private sealed record EmployeeResponse(Guid Id, Guid OrganizationId, Guid DepartmentId, string DisplayName, string? Email, bool IsActive);

    private sealed record ShiftTypeResponse(Guid Id, Guid OrganizationId, string Name, string? Code, bool IsActive);

    private sealed record ShiftAssignmentResponse(
        Guid Id,
        Guid OrganizationId,
        Guid EmployeeId,
        Guid ShiftTypeId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        string Status);

    private sealed record MonthCalendarResponse(
        IReadOnlyList<CalendarAssignmentResponse> Assignments,
        IReadOnlyList<CalendarLeaveResponse> Leaves);

    private sealed record CalendarAssignmentResponse(
        Guid Id,
        Guid EmployeeId,
        Guid ShiftTypeId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        string Status);

    private sealed record CalendarLeaveResponse(
        Guid Id,
        Guid EmployeeId,
        DateOnly StartOn,
        DateOnly EndOn,
        string Status);

    private sealed record ErrorBody(string Code, string Message);
}
