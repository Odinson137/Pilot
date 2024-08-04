using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Test.Receiver.IntegrationTests.Factories;
using Test.Receiver.IntegrationTests.TestSettings;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class ProjectTaskTests : BaseModelReceiverIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}