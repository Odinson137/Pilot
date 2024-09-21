using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class HistoryActionTests : BaseModelReceiverIntegrationTest<HistoryAction, HistoryActionDto>
{
    /// <inheritdoc />
    public HistoryActionTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory)
        : base(receiverFactory, identityFactory)
    {
    }
}