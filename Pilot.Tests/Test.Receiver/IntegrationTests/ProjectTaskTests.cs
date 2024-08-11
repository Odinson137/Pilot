using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Base.IntegrationBase;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class ProjectTaskTests : BaseModelReceiverIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}