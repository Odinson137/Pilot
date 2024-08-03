using Pilot.Contracts.DTO.ModelDto;
using Test.Receiver.IntegrationTests.Factories;
using Test.Receiver.IntegrationTests.TestSettings;
using File = Pilot.Contracts.Models.File;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class FileReceiverTests : BaseModelReceiverIntegrationTest<File, FileDto>
{
    /// <inheritdoc />
    public FileReceiverTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}