using FluentAssertions;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Leaves;
using ShiftFlow.Domain.Rules;
using ShiftFlow.Domain.ShiftAssignments;

namespace ShiftFlow.UnitTests.Domain;

public class LeaveAndHr02Tests
{
    private static readonly Guid OrgId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid EmployeeId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ShiftTypeId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    [Fact]
    public void Leave_rechaza_EndOn_anterior_a_StartOn()
    {
        var act = () => Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            new DateOnly(2026, 8, 16),
            new DateOnly(2026, 8, 15));

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-LEA-03");
    }

    [Fact]
    public void Leave_rechaza_empleado_inactivo()
    {
        var act = () => Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: false,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-LEA-02");
    }

    [Fact]
    public void Cancel_solo_sobre_Active()
    {
        var leave = Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));

        leave.Cancel();
        leave.Status.Should().Be(LeaveStatus.Cancelled);

        var act = () => leave.Cancel();
        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-LEA-05");
    }

    [Fact]
    public void HR02_rechaza_asignacion_bajo_leave()
    {
        var leave = Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));

        var candidate = CreateAssigned(2026, 8, 15, 10, 14);
        var engine = new RuleEngine();

        var violations = engine.Evaluate(candidate, [], [leave]);

        violations.Should().ContainSingle(v => v.Code == "HR-02");
    }

    [Fact]
    public void HR02_permite_asignacion_fuera_del_leave()
    {
        var leave = Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));

        var candidate = CreateAssigned(2026, 8, 16, 10, 14);
        var engine = new RuleEngine();

        var violations = engine.Evaluate(candidate, [], [leave]);

        violations.Should().BeEmpty();
    }

    [Fact]
    public void HR02_no_aplica_si_leave_cancelado()
    {
        var leave = Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));
        leave.Cancel();

        var candidate = CreateAssigned(2026, 8, 15, 10, 14);
        var engine = new RuleEngine();

        var violations = engine.Evaluate(candidate, [], [leave]);

        violations.Should().BeEmpty();
    }

    [Fact]
    public void HR01_y_HR02_son_distinguibles()
    {
        var engine = new RuleEngine();
        var existing = CreateAssigned(2026, 8, 10, 10, 14);
        var overlapCandidate = CreateAssigned(2026, 8, 10, 12, 16);
        var leave = Leave.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 15));
        var leaveCandidate = CreateAssigned(2026, 8, 15, 10, 14);

        engine.Evaluate(overlapCandidate, [existing], []).Should().ContainSingle(v => v.Code == "HR-01");
        engine.Evaluate(leaveCandidate, [], [leave]).Should().ContainSingle(v => v.Code == "HR-02");
    }

    private static ShiftAssignment CreateAssigned(int year, int month, int day, int startHour, int endHour) =>
        ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: true,
            new DateTimeOffset(year, month, day, startHour, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(year, month, day, endHour, 0, 0, TimeSpan.Zero));
}
