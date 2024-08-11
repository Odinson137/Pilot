using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;

namespace Test.Api.IntegrationTests;

public class ProjectTaskTests : BaseModelIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) 
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}