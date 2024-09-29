using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class TeamTests : BaseModelReceiverIntegrationTest<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory) : base(
        workerTestWorkerFactory, identityFactory)
    {
    }
}