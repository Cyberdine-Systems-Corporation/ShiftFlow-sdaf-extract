using FluentAssertions;
using ShiftFlow.Domain;

namespace ShiftFlow.UnitTests;

public class DomainAssemblyTests
{
    [Fact]
    public void AssemblyMarker_pertenece_al_ensamblado_Domain()
    {
        typeof(AssemblyMarker).Assembly.GetName().Name.Should().Be("ShiftFlow.Domain");
    }
}
