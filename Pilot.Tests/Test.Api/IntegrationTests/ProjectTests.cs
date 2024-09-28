using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class ProjectTests : BaseModelIntegrationTest<Project, ProjectDto>
{
    /// <inheritdoc />
    public ProjectTests(ApiTestApiFactory apiFactory, ApiTestWorkerFactory workerFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, workerFactory, identityFactory)
    {
    }
}