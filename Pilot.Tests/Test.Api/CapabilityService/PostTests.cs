using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.CapabilityService.Factory;

namespace Test.Api.CapabilityService;

public class PostTests : CapabilityTests<Post, PostDto>
{
    /// <inheritdoc />
    public PostTests(
        CapabilityTestApiFactory apiFactory, 
        CapabilityTestIdentityFactory identityFactory, 
        CapabilityTestCapabilityFactory capabilityFactory, 
        CapabilityTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, capabilityFactory, storageFactory)
    {
    }
}