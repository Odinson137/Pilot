using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.CapabilityService.Factory;

namespace Test.Api.CapabilityService;

public class SkillTests : CapabilityTests<Post, PostDto>
{
    /// <inheritdoc />
    public SkillTests(
        CapabilityTestApiFactory apiFactory, 
        CapabilityTestIdentityFactory identityFactory, 
        CapabilityTestCapabilityFactory capabilityFactory, 
        CapabilityTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, capabilityFactory, storageFactory)
    {
    }
}