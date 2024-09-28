using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class SkillTests : BaseModelReceiverIntegrationTest<Skill, SkillDto>
{
    /// <inheritdoc />
    public SkillTests(CapabilityTestCapabilityFactory factory) :
        base(factory)
    {
    }
}