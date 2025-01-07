using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.WorkerService.Factory;
using Xunit.Abstractions;

namespace Test.Api.WorkerService;

public class TaskInfoTests : WorkerTests<TaskInfo, TaskInfoDto>
{
    /// <inheritdoc />
    public TaskInfoTests(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory, 
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory, ITestOutputHelper testOutputHelper)
        : base(apiFactory, identityFactory, workerFactory, storageFactory, testOutputHelper)
    {
    }
}