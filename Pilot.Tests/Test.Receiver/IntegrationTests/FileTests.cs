using Pilot.Contracts.DTO.ModelDto;
using Test.Receiver.IntegrationTests.Factories;
using File = Pilot.Receiver.Models.File;

namespace Test.Receiver.IntegrationTests;

public class FileTests : BaseModelReceiverIntegrationTest<File, FileDto>
{
    /// <inheritdoc />
    public FileTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
}