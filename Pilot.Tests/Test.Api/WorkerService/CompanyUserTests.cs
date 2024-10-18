using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests;
using Test.Api.IntegrationTests.Factories;
using Test.Api.WorkerService.Factory;

namespace Test.Api.WorkerService;

public class CompanyUserTests : WorkerTests<CompanyUser, CompanyUserDto>
{
    /// <inheritdoc />
    public CompanyUserTests(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory, 
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, workerFactory, storageFactory)
    {
    }
}