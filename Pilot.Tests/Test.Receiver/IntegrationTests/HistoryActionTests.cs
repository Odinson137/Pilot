using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Test.Receiver.IntegrationTests.Factories;
using Test.Receiver.IntegrationTests.TestSettings;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class HistoryActionTests : BaseModelReceiverIntegrationTest<HistoryAction, HistoryActionDto>
{
    /// <inheritdoc />
    public HistoryActionTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}