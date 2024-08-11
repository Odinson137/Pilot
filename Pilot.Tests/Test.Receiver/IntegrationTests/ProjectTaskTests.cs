using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class ProjectTaskTests : BaseModelReceiverIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}