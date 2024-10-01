using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class ProjectTests : BaseModelReceiverIntegrationTest<Project, ProjectDto>
{
    /// <inheritdoc />
    public ProjectTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory, WorkerTestStorageFactory storageFactory) :
        base(workerTestWorkerFactory, identityFactory, storageFactory)
    {
    }
}