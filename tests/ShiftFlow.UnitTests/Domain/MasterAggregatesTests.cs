using FluentAssertions;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Departments;
using ShiftFlow.Domain.Employees;
using ShiftFlow.Domain.Organizations;
using ShiftFlow.Domain.ShiftTypes;

namespace ShiftFlow.UnitTests.Domain;

public class MasterAggregatesTests
{
    [Fact]
    public void Organization_rechaza_nombre_vacio()
    {
        var act = () => Organization.Create("   ");
        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-ORG-01");
    }

    [Fact]
    public void Department_exige_organization_activa()
    {
        var orgId = Guid.NewGuid();
        var act = () => Department.Create(orgId, "Urgencias", organizationIsActive: false);
        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-DEP-01");
    }

    [Fact]
    public void Employee_rechaza_departamento_de_otra_organization()
    {
        var orgA = Guid.NewGuid();
        var orgB = Guid.NewGuid();
        var deptId = Guid.NewGuid();

        var act = () => Employee.Create(
            orgA,
            deptId,
            departmentOrganizationId: orgB,
            departmentIsActive: true,
            displayName: "Ana",
            email: null);

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-EMP-01");
    }

    [Fact]
    public void Employee_exige_display_name()
    {
        var orgId = Guid.NewGuid();
        var act = () => Employee.Create(
            orgId,
            Guid.NewGuid(),
            orgId,
            departmentIsActive: true,
            displayName: " ",
            email: null);

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-EMP-02");
    }

    [Fact]
    public void ShiftType_rechaza_ventana_overnight()
    {
        var orgId = Guid.NewGuid();
        var act = () => ShiftType.Create(
            orgId,
            organizationIsActive: true,
            name: "Noche",
            code: "NOC",
            defaultStartTime: new TimeOnly(22, 0),
            defaultEndTime: new TimeOnly(6, 0));

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-STT-04");
    }

    [Fact]
    public void ShiftType_exige_organization_activa()
    {
        var act = () => ShiftType.Create(
            Guid.NewGuid(),
            organizationIsActive: false,
            name: "Mañana",
            code: null,
            defaultStartTime: null,
            defaultEndTime: null);

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-STT-01");
    }

    [Fact]
    public void Aggregates_crean_activos_por_defecto()
    {
        var org = Organization.Create("Hospital Demo");
        var dept = Department.Create(org.Id, "Urgencias", org.IsActive);
        var emp = Employee.Create(org.Id, dept.Id, dept.OrganizationId, dept.IsActive, "Ana Pérez", "ana@demo.local");
        var shiftType = ShiftType.Create(
            org.Id,
            org.IsActive,
            "Mañana",
            "MAN",
            new TimeOnly(8, 0),
            new TimeOnly(15, 0));

        org.IsActive.Should().BeTrue();
        dept.IsActive.Should().BeTrue();
        emp.IsActive.Should().BeTrue();
        emp.Email.Should().Be("ana@demo.local");
        shiftType.IsActive.Should().BeTrue();
        shiftType.Code.Should().Be("MAN");
    }
}
