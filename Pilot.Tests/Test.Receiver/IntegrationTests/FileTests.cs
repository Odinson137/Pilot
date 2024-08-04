using Pilot.Contracts.DTO.ModelDto;
using Test.Receiver.IntegrationTests.Factories;
using Test.Receiver.IntegrationTests.TestSettings;
using File = Pilot.Contracts.Models.File;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class FileTests : BaseModelReceiverIntegrationTest<File, FileDto>
{
    /// <inheritdoc />
    public FileTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}