using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Base.IntegrationBase;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class HistoryActionTests : BaseModelReceiverIntegrationTest<HistoryAction, HistoryActionDto>
{
    /// <inheritdoc />
    public HistoryActionTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}