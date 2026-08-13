using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShiftFlow.Application.Auth;

namespace ShiftFlow.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class MasterDataApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ShiftFlowApiFactory _factory;

    public MasterDataApiTests(ShiftFlowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ACC_S1_03_alta_Organization_Department_Employee_ShiftType()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();

        OrganizationResponse? org = await CreateOrganizationAsync(client, "Hospital Norte");
        DepartmentResponse? dept = await CreateDepartmentAsync(client, org.Id, "Urgencias");
        EmployeeResponse? emp = await CreateEmployeeAsync(client, org.Id, dept.Id, "Ana Pérez", "ana@norte.local");
        ShiftTypeResponse? shiftType = await CreateShiftTypeAsync(
            client,
            org.Id,
            "Mañana",
            "MAN",
            "08:00:00",
            "15:00:00");

        List<OrganizationResponse>? orgs = await client.GetFromJsonAsync<List<OrganizationResponse>>("/api/organizations", JsonOptions);
        orgs.Should().Contain(o => o.Id == org.Id && o.Name == "Hospital Norte");

        List<DepartmentResponse>? depts = await client.GetFromJsonAsync<List<DepartmentResponse>>(
            $"/api/organizations/{org.Id}/departments",
            JsonOptions);
        depts.Should().Contain(d => d.Id == dept.Id && d.Name == "Urgencias");

        List<EmployeeResponse>? emps = await client.GetFromJsonAsync<List<EmployeeResponse>>(
            $"/api/organizations/{org.Id}/employees",
            JsonOptions);
        emps.Should().Contain(e => e.Id == emp.Id && e.DisplayName == "Ana Pérez");

        List<ShiftTypeResponse>? shiftTypes = await client.GetFromJsonAsync<List<ShiftTypeResponse>>(
            $"/api/organizations/{org.Id}/shift-types",
            JsonOptions);
        shiftTypes.Should().Contain(s => s.Id == shiftType.Id && s.Name == "Mañana" && s.Code == "MAN");
    }

    [Fact]
    public async Task ACC_S1_04_unicidad_departamento_case_insensitive()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        OrganizationResponse? org = await CreateOrganizationAsync(client, "Hospital Unicidad");
        await CreateDepartmentAsync(client, org.Id, "Urgencias");

        HttpResponseMessage? duplicate = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/departments",
            new { name = "urgencias" });

        duplicate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await duplicate.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("INV-DEP-02");
    }

    [Fact]
    public async Task ACC_S1_05_employee_no_cruza_organizations()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        OrganizationResponse? orgA = await CreateOrganizationAsync(client, "Org A");
        OrganizationResponse? orgB = await CreateOrganizationAsync(client, "Org B");
        DepartmentResponse? deptA = await CreateDepartmentAsync(client, orgA.Id, "Dept A");
        DepartmentResponse? deptB = await CreateDepartmentAsync(client, orgB.Id, "Dept B");

        HttpResponseMessage? createCross = await client.PostAsJsonAsync(
            $"/api/organizations/{orgA.Id}/employees",
            new { departmentId = deptB.Id, displayName = "Cruzado", email = (string?)null });

        createCross.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? createBody = await createCross.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        createBody!.Code.Should().Be("INV-EMP-01");

        EmployeeResponse? employee = await CreateEmployeeAsync(client, orgA.Id, deptA.Id, "Valido", null);
        HttpResponseMessage? moveCross = await client.PutAsJsonAsync(
            $"/api/employees/{employee.Id}",
            new { departmentId = deptB.Id, displayName = "Valido", email = (string?)null });

        moveCross.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? moveBody = await moveCross.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        moveBody!.Code.Should().Be("INV-EMP-01");
    }

    [Fact]
    public async Task ACC_S1_06_ShiftType_horario_overnight_rechazado()
    {
        HttpClient? client = await CreateAuthenticatedClientAsync();
        OrganizationResponse? org = await CreateOrganizationAsync(client, "Hospital Overnight");

        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/shift-types",
            new
            {
                name = "Noche",
                code = "NOC",
                defaultStartTime = "22:00:00",
                defaultEndTime = "06:00:00"
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ErrorBody? body = await response.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("INV-STT-04");
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

    private static async Task<OrganizationResponse> CreateOrganizationAsync(HttpClient client, string name)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync("/api/organizations", new { name });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        OrganizationResponse? body = await response.Content.ReadFromJsonAsync<OrganizationResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
    }

    private static async Task<DepartmentResponse> CreateDepartmentAsync(
        HttpClient client,
        Guid organizationId,
        string name)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/departments",
            new { name });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        DepartmentResponse? body = await response.Content.ReadFromJsonAsync<DepartmentResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
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
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        EmployeeResponse? body = await response.Content.ReadFromJsonAsync<EmployeeResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
    }

    private static async Task<ShiftTypeResponse> CreateShiftTypeAsync(
        HttpClient client,
        Guid organizationId,
        string name,
        string? code,
        string? defaultStartTime,
        string? defaultEndTime)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/shift-types",
            new { name, code, defaultStartTime, defaultEndTime });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        ShiftTypeResponse? body = await response.Content.ReadFromJsonAsync<ShiftTypeResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
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

    private sealed record ErrorBody(string Error, string Code);
}
