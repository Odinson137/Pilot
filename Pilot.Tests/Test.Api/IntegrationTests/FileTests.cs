using Pilot.Contracts.DTO.ModelDto;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;
using File = Pilot.Receiver.Models.File;

namespace Test.Api.IntegrationTests;

public class FileTests : BaseModelIntegrationTest<File, FileDto>
{
    /// <inheritdoc />
    public FileTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) 
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}