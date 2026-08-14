using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class RuleExplainApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public RuleExplainApiTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ACC_S3_X01_explicacion_HR01_sin_mutar()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Explain HR01");

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
        RuleErrorBody? assignBody = await overlap.Content.ReadFromJsonAsync<RuleErrorBody>(JsonOptions);
        assignBody!.Code.Should().Be("HR-01");
        assignBody.Title.Should().NotBeNullOrWhiteSpace();
        assignBody.Body.Should().Contain("solape");
        assignBody.MutatesSchedule.Should().BeFalse();

        HttpResponseMessage? explain = await client.GetAsync("/api/rules/explain?code=HR-01");
        explain.StatusCode.Should().Be(HttpStatusCode.OK);
        RuleExplanationBody? explanation = await explain.Content.ReadFromJsonAsync<RuleExplanationBody>(JsonOptions);
        explanation!.Code.Should().Be("HR-01");
        explanation.Title.Should().Contain("Solape");
        explanation.Body.Should().Contain("solape");
        explanation.MutatesSchedule.Should().BeFalse();

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().ContainSingle();
    }

    [Fact]
    public async Task ACC_S3_X02_explicacion_HR02()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Explain HR02");

        HttpResponseMessage? leave = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/leaves",
            new { employeeId = emp.Id, startOn = "2026-08-15", endOn = "2026-08-15" });
        leave.EnsureSuccessStatusCode();

        DateTimeOffset day = new DateTimeOffset(2026, 8, 15, 0, 0, 0, TimeSpan.Zero);
        HttpResponseMessage? blocked = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(10),
                endAt = day.AddHours(14)
            });

        blocked.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        RuleErrorBody? assignBody = await blocked.Content.ReadFromJsonAsync<RuleErrorBody>(JsonOptions);
        assignBody!.Code.Should().Be("HR-02");
        assignBody.Body.Should().Contain("ausencia");

        HttpResponseMessage? explain = await client.GetAsync("/api/rules/explain?code=HR-02");
        explain.EnsureSuccessStatusCode();
        RuleExplanationBody? explanation = await explain.Content.ReadFromJsonAsync<RuleExplanationBody>(JsonOptions);
        explanation!.Body.Should().Match(b => b.Contains("ausencia") || b.Contains("leave"));
        explanation.MutatesSchedule.Should().BeFalse();

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().BeEmpty();
    }

    [Fact]
    public async Task ACC_S3_X03_explicacion_HR03()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Explain HR03");

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
        RuleErrorBody? assignBody = await tooSoon.Content.ReadFromJsonAsync<RuleErrorBody>(JsonOptions);
        assignBody!.Code.Should().Be("HR-03");
        assignBody.Body.Should().Contain("descanso");

        HttpResponseMessage? explain = await client.GetAsync("/api/rules/explain?code=HR-03");
        explain.EnsureSuccessStatusCode();
        RuleExplanationBody? explanation = await explain.Content.ReadFromJsonAsync<RuleExplanationBody>(JsonOptions);
        explanation!.Body.Should().Contain("descanso");
        explanation.MutatesSchedule.Should().BeFalse();

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().ContainSingle();
    }

    [Fact]
    public async Task ACC_S3_X04_codigo_no_soportado()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        HttpResponseMessage? explain = await client.GetAsync("/api/rules/explain?code=HR-99");
        explain.StatusCode.Should().Be(HttpStatusCode.OK);
        RuleExplanationBody? explanation = await explain.Content.ReadFromJsonAsync<RuleExplanationBody>(JsonOptions);
        explanation!.Code.Should().Be("HR-99");
        explanation.Title.Should().Contain("no soportado");
        explanation.Body.Should().Contain("no es una hard rule");
        explanation.MutatesSchedule.Should().BeFalse();
    }

    [Fact]
    public async Task ACC_S3_X05_anonimo_rechazado()
    {
        HttpClient? anonymous = _factory.CreateClient();
        HttpResponseMessage? response = await anonymous.GetAsync("/api/rules/explain?code=HR-01");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ACC_S3_X06_stub_no_bypassea_rule_engine()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        (OrganizationResponse org, EmployeeResponse emp, ShiftTypeResponse shiftType) = await SeedOrgAsync(client, "Org Explain no bypass");

        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(10), day.AddHours(14));

        HttpResponseMessage? explain = await client.GetAsync("/api/rules/explain?code=HR-01");
        explain.EnsureSuccessStatusCode();

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
        RuleErrorBody? body = await overlap.Content.ReadFromJsonAsync<RuleErrorBody>(JsonOptions);
        body!.Code.Should().Be("HR-01");

        MonthCalendarResponse? calendar = await client.GetFromJsonAsync<MonthCalendarResponse>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar!.Assignments.Should().ContainSingle();
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
        HttpResponseMessage? deptResponse = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/departments",
            new { name = "Dept" });
        deptResponse.EnsureSuccessStatusCode();
        DepartmentResponse? dept = await deptResponse.Content.ReadFromJsonAsync<DepartmentResponse>(JsonOptions);

        HttpResponseMessage? empResponse = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/employees",
            new { departmentId = dept!.Id, displayName = "Ana", email = (string?)null });
        empResponse.EnsureSuccessStatusCode();
        EmployeeResponse? emp = await empResponse.Content.ReadFromJsonAsync<EmployeeResponse>(JsonOptions);

        HttpResponseMessage? stResponse = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/shift-types",
            new { name = "Mañana", code = "MAN", defaultStartTime = (string?)null, defaultEndTime = (string?)null });
        stResponse.EnsureSuccessStatusCode();
        ShiftTypeResponse? shiftType = await stResponse.Content.ReadFromJsonAsync<ShiftTypeResponse>(JsonOptions);

        return (org, emp!, shiftType!);
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

    private sealed record RuleExplanationBody(string Code, string Title, string Body, bool MutatesSchedule);

    private sealed record RuleErrorBody(
        string Error,
        string Code,
        string? Title,
        string? Body,
        bool MutatesSchedule);
}
