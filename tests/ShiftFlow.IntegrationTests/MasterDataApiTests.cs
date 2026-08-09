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
    public async Task ACC_S1_03_alta_Organization_Department_Employee()
    {
        var client = await CreateAuthenticatedClientAsync();

        var org = await CreateOrganizationAsync(client, "Hospital Norte");
        var dept = await CreateDepartmentAsync(client, org.Id, "Urgencias");
        var emp = await CreateEmployeeAsync(client, org.Id, dept.Id, "Ana Pérez", "ana@norte.local");

        var orgs = await client.GetFromJsonAsync<List<OrganizationResponse>>("/api/organizations", JsonOptions);
        orgs.Should().Contain(o => o.Id == org.Id && o.Name == "Hospital Norte");

        var depts = await client.GetFromJsonAsync<List<DepartmentResponse>>(
            $"/api/organizations/{org.Id}/departments",
            JsonOptions);
        depts.Should().Contain(d => d.Id == dept.Id && d.Name == "Urgencias");

        var emps = await client.GetFromJsonAsync<List<EmployeeResponse>>(
            $"/api/organizations/{org.Id}/employees",
            JsonOptions);
        emps.Should().Contain(e => e.Id == emp.Id && e.DisplayName == "Ana Pérez");
    }

    [Fact]
    public async Task ACC_S1_04_unicidad_departamento_case_insensitive()
    {
        var client = await CreateAuthenticatedClientAsync();
        var org = await CreateOrganizationAsync(client, "Hospital Unicidad");
        await CreateDepartmentAsync(client, org.Id, "Urgencias");

        var duplicate = await client.PostAsJsonAsync(
            $"/api/organizations/{org.Id}/departments",
            new { name = "urgencias" });

        duplicate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await duplicate.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        body!.Code.Should().Be("INV-DEP-02");
    }

    [Fact]
    public async Task ACC_S1_05_employee_no_cruza_organizations()
    {
        var client = await CreateAuthenticatedClientAsync();
        var orgA = await CreateOrganizationAsync(client, "Org A");
        var orgB = await CreateOrganizationAsync(client, "Org B");
        var deptA = await CreateDepartmentAsync(client, orgA.Id, "Dept A");
        var deptB = await CreateDepartmentAsync(client, orgB.Id, "Dept B");

        var createCross = await client.PostAsJsonAsync(
            $"/api/organizations/{orgA.Id}/employees",
            new { departmentId = deptB.Id, displayName = "Cruzado", email = (string?)null });

        createCross.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var createBody = await createCross.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        createBody!.Code.Should().Be("INV-EMP-01");

        var employee = await CreateEmployeeAsync(client, orgA.Id, deptA.Id, "Valido", null);
        var moveCross = await client.PutAsJsonAsync(
            $"/api/employees/{employee.Id}",
            new { departmentId = deptB.Id, displayName = "Valido", email = (string?)null });

        moveCross.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var moveBody = await moveCross.Content.ReadFromJsonAsync<ErrorBody>(JsonOptions);
        moveBody!.Code.Should().Be("INV-EMP-01");
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

    private static async Task<OrganizationResponse> CreateOrganizationAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("/api/organizations", new { name });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<OrganizationResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
    }

    private static async Task<DepartmentResponse> CreateDepartmentAsync(
        HttpClient client,
        Guid organizationId,
        string name)
    {
        var response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/departments",
            new { name });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<DepartmentResponse>(JsonOptions);
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
        var response = await client.PostAsJsonAsync(
            $"/api/organizations/{organizationId}/employees",
            new { departmentId, displayName, email });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<EmployeeResponse>(JsonOptions);
        body.Should().NotBeNull();
        return body!;
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

    private sealed record ErrorBody(string Error, string Code);
}
