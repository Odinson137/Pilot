using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class TeamTests : BaseModelIntegrationTest<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}