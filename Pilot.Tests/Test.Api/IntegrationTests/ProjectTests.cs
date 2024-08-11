using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;

namespace Test.Api.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class ProjectTests : BaseModelIntegrationTest<Project, ProjectDto>
{
    /// <inheritdoc />
    public ProjectTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) 
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}