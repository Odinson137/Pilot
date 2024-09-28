using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class ProjectTaskTests : BaseModelIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(ApiTestApiFactory apiFactory, ApiTestWorkerFactory workerFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, workerFactory, identityFactory)
    {
    }
}