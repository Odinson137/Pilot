using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.WorkerService.Factory;
using Xunit.Abstractions;

namespace Test.Api.WorkerService;

public class TeamTests : WorkerTests<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory, 
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory, ITestOutputHelper testOutputHelper)
        : base(apiFactory, identityFactory, workerFactory, storageFactory, testOutputHelper)
    {
    }
}