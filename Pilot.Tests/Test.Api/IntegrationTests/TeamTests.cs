using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;

namespace Test.Api.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class TeamTests : BaseModelIntegrationTest<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) 
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}