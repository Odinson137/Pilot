using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class TaskInfoTests : BaseModelReceiverIntegrationTest<TaskInfo, TaskInfoDto>
{
    /// <inheritdoc />
    public TaskInfoTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }
}