using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class UserSkillTests(CapabilityTestCapabilityFactory factory)
    : BaseModelReceiverIntegrationTest<UserSkill, UserSkillDto>(factory);