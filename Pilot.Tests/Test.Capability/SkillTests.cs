using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;

namespace Test.Capability;

public class SkillTests : BaseModelReceiverIntegrationTest<Skill, SkillDto>
{
    /// <inheritdoc />
    public SkillTests(CapabilityTestCapabilityFactory factory) :
        base(factory)
    {
    }
}