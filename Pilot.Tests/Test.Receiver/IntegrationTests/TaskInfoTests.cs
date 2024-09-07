using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class TaskInfoTests : BaseModelReceiverIntegrationTest<TaskInfo, TaskInfoDto>
{
    /// <inheritdoc />
    public TaskInfoTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }
}