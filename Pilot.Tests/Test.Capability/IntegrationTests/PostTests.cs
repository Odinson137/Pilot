using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class PostSkillTests(CapabilityTestCapabilityFactory factory)
    : BaseModelReceiverIntegrationTest<Post, PostDto>(factory);