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
        var client = await CreateAuthenticatedClientAsync();
        var org = await CreateOrganizationAsync(client, "Org Calendario");

        var response = await client.GetAsync($"/api/organizations/{org.Id}/calendar?year=2026&month=8");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<CalendarItem>>(JsonOptions);
        items.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task ACC_S2_02_asignacion_valida_visible_en_calendario()
    {
        var client = await CreateAuthenticatedClientAsync();
        var (org, emp, shiftType) = await SeedOrgAsync(client, "Org Assign OK");

        var start = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        var end = start.AddHours(4);
        var assign = await AssignAsync(client, org.Id, emp.Id, shiftType.Id, start, end);

        assign.Status.Should().Be("Assigned");

        var calendar = await client.GetFromJsonAsync<List<CalendarItem>>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);

        calendar.Should().ContainSingle(x =>
            x.Id == assign.Id
            && x.EmployeeId == emp.Id
            && x.ShiftTypeId == shiftType.Id);
    }

    [Fact]
    public async Task ACC_S2_03_rechazo_por_solape_HR01()
    {
        var client = await CreateAuthenticatedClientAsync();
        var (org, emp, shiftType) = await SeedOrgAsync(client, "Org Solape");

        var day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(10), day.AddHours(14));

        var overlap = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day.AddHours(12),
                endAt = day.AddHours(16)
            });

        overlap.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await overlap.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("HR-01");

        var calendar = await client.GetFromJsonAsync<List<CalendarItem>>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar.Should().ContainSingle();
    }

    [Fact]
    public async Task ACC_S2_04_turnos_adyacentes_permitidos()
    {
        var client = await CreateAuthenticatedClientAsync();
        var (org, emp, shiftType) = await SeedOrgAsync(client, "Org Adyacentes");

        var day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day.AddHours(10), day.AddHours(14));
        var second = await AssignAsync(
            client,
            org.Id,
            emp.Id,
            shiftType.Id,
            day.AddHours(14),
            day.AddHours(18));

        second.Status.Should().Be("Assigned");

        var calendar = await client.GetFromJsonAsync<List<CalendarItem>>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar.Should().HaveCount(2);
    }

    [Fact]
    public async Task ACC_S2_05_rechazo_shift_type_inactivo()
    {
        var client = await CreateAuthenticatedClientAsync();
        var (org, emp, shiftType) = await SeedOrgAsync(client, "Org Tipo Inactivo");

        var deactivate = await client.PutAsJsonAsync(
            $"/api/shift-types/{shiftType.Id}/active",
            new { isActive = false });
        deactivate.EnsureSuccessStatusCode();

        var day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        var response = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/assignments",
            new
            {
                employeeId = emp.Id,
                shiftTypeId = shiftType.Id,
                startAt = day,
                endAt = day.AddHours(4)
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("INV-ASN-03");
    }

    [Fact]
    public async Task ACC_S2_06_cancelar_asignacion()
    {
        var client = await CreateAuthenticatedClientAsync();
        var (org, emp, shiftType) = await SeedOrgAsync(client, "Org Cancel");

        var day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        var assign = await AssignAsync(client, org.Id, emp.Id, shiftType.Id, day, day.AddHours(4));

        var cancel = await client.PostAsync($"/api/assignments/{assign.Id}/cancel", null);
        cancel.StatusCode.Should().Be(HttpStatusCode.OK);

        var calendar = await client.GetFromJsonAsync<List<CalendarItem>>(
            $"/api/organizations/{org.Id}/calendar?year=2026&month=8",
            JsonOptions);
        calendar.Should().BeEmpty();
    }

    [Fact]
    public async Task ACC_S2_07_escritura_anonima_rechazada()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync(
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
        var client = _factory.CreateClient(new() { HandleCookies = true });
        var login = await client.PostAsJsonAsync("/api/auth/login", new
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
        var org = await CreateOrganizationAsync(client, orgName);
        var dept = await CreateDepartmentAsync(client, org.Id, "Dept");
        var emp = await CreateEmployeeAsync(client, org.Id, dept.Id, "Ana", null);
        var shiftType = await CreateShiftTypeAsync(client, org.Id, "Mañana", "MAN");
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
        var response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/assignments",
            new { employeeId, shiftTypeId, startAt, endAt });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<ShiftAssignmentResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
    }

    private static async Task<OrganizationResponse> CreateOrganizationAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("/api/organizations", new { name });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<OrganizationResponse>(JsonOptions))!;
    }

    private static async Task<DepartmentResponse> CreateDepartmentAsync(
        HttpClient client,
        Guid organizationId,
        string name)
    {
        var response = await client.PostAsJsonAsync(
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
        var response = await client.PostAsJsonAsync(
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
        var response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/shift-types",
            new { name, code, defaultStartTime = (string?)null, defaultEndTime = (string?)null });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ShiftTypeResponse>(JsonOptions))!;
    }

    private sealed record OrganizationResponse(Guid Id, string Name, bool IsActive);

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

    private sealed record CalendarItem(
        Guid Id,
        Guid EmployeeId,
        string EmployeeDisplayName,
        Guid ShiftTypeId,
        string ShiftTypeName,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt);

    private sealed record ErrorBody(string Error, string Code);
}
