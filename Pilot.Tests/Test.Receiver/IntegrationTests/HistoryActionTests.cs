using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class HistoryActionTests : BaseModelReceiverIntegrationTest<HistoryAction, HistoryActionDto>
{
    /// <inheritdoc />
    public HistoryActionTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory)
        : base(receiverFactory, identityFactory)
    {
    }
}