using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class ProjectTests : BaseModelIntegrationTest<Project, ProjectDto>
{
    /// <inheritdoc />
    public ProjectTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}