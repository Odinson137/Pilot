using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.WorkerService.Factory;
using Xunit.Abstractions;

namespace Test.Api.WorkerService;

public class ProjectTests : WorkerTests<Project, ProjectDto>
{
    /// <inheritdoc />
    public ProjectTests(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory, 
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory, ITestOutputHelper testOutputHelper)
        : base(apiFactory, identityFactory, workerFactory, storageFactory, testOutputHelper)
    {
    }
}