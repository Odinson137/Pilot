using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class TeamTests : BaseModelIntegrationTest<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(ApiTestApiFactory apiFactory, ApiTestWorkerFactory workerFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, workerFactory, identityFactory)
    {
    }
}