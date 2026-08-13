using FluentAssertions;
using ShiftFlow.Domain.Common;
using ShiftFlow.Domain.Rules;
using ShiftFlow.Domain.ShiftAssignments;

namespace ShiftFlow.UnitTests.Domain;

public class ShiftAssignmentAndRulesTests
{
    private static readonly Guid OrgId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid EmployeeId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ShiftTypeId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    [Fact]
    public void ShiftAssignment_rechaza_tipo_inactivo()
    {
        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        Func<ShiftAssignment>? act = () => ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: false,
            day,
            day.AddHours(4));

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-ASN-03");
    }

    [Fact]
    public void ShiftAssignment_rechaza_intervalo_invalido()
    {
        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        Func<ShiftAssignment>? act = () => ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: true,
            day,
            day);

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-ASN-04");
    }

    [Fact]
    public void ShiftAssignment_rechaza_empleado_de_otra_organization()
    {
        Guid otherOrg = Guid.NewGuid();
        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 8, 0, 0, TimeSpan.Zero);
        Func<ShiftAssignment>? act = () => ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            otherOrg,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: true,
            day,
            day.AddHours(4));

        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-ASN-01");
    }

    [Fact]
    public void Cancel_solo_sobre_Assigned()
    {
        ShiftAssignment? assignment = CreateAssigned(8, 12);
        assignment.Cancel();
        assignment.Status.Should().Be(ShiftAssignmentStatus.Cancelled);

        Action? act = () => assignment.Cancel();
        act.Should().Throw<DomainException>().Which.Code.Should().Be("INV-ASN-06");
    }

    [Fact]
    public void HR01_rechaza_solape()
    {
        ShiftAssignment? existing = CreateAssigned(10, 14);
        ShiftAssignment? candidate = CreateAssigned(12, 16);
        RuleEngine engine = new RuleEngine();

        IReadOnlyList<RuleViolation>? violations = engine.Evaluate(candidate, [existing]);

        violations.Should().ContainSingle(v => v.Code == "HR-01");
    }

    [Fact]
    public void HR01_permite_turnos_adyacentes()
    {
        ShiftAssignment? existing = CreateAssigned(10, 14);
        ShiftAssignment? candidate = CreateAssigned(14, 18);
        RuleEngine engine = new RuleEngine();

        IReadOnlyList<RuleViolation>? violations = engine.Evaluate(candidate, [existing]);

        violations.Should().BeEmpty();
    }

    [Fact]
    public void HR03_rechaza_gap_inferior_al_umbral()
    {
        ShiftAssignment? existing = CreateAssigned(8, 16);
        ShiftAssignment? candidate = CreateAssigned(16, 20);
        RuleEngine engine = new RuleEngine();

        IReadOnlyList<RuleViolation>? violations = engine.Evaluate(
            candidate,
            [existing],
            minimumRest: TimeSpan.FromMinutes(660));

        violations.Should().ContainSingle(v => v.Code == "HR-03");
    }

    [Fact]
    public void HR03_permite_gap_igual_al_umbral()
    {
        DateTimeOffset day = new DateTimeOffset(2026, 8, 10, 0, 0, 0, TimeSpan.Zero);
        ShiftAssignment existing = ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: true,
            day.AddHours(8),
            day.AddHours(16));
        ShiftAssignment candidate = ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: true,
            day.AddHours(16).AddMinutes(660),
            day.AddHours(16).AddMinutes(660 + 240));
        RuleEngine engine = new RuleEngine();

        IReadOnlyList<RuleViolation>? violations = engine.Evaluate(
            candidate,
            [existing],
            minimumRest: TimeSpan.FromMinutes(660));

        violations.Should().BeEmpty();
    }

    [Fact]
    public void HR03_no_aplica_si_umbral_cero()
    {
        ShiftAssignment? existing = CreateAssigned(10, 14);
        ShiftAssignment? candidate = CreateAssigned(14, 18);
        RuleEngine engine = new RuleEngine();

        IReadOnlyList<RuleViolation>? violations = engine.Evaluate(
            candidate,
            [existing],
            minimumRest: TimeSpan.Zero);

        violations.Should().BeEmpty();
    }

    private static ShiftAssignment CreateAssigned(int startHour, int endHour) =>
        ShiftAssignment.Create(
            OrgId,
            EmployeeId,
            OrgId,
            employeeIsActive: true,
            ShiftTypeId,
            OrgId,
            shiftTypeIsActive: true,
            new DateTimeOffset(2026, 8, 10, startHour, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 8, 10, endHour, 0, 0, TimeSpan.Zero));
}
