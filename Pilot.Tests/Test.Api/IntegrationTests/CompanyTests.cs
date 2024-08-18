using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class CompanyTests : BaseModelIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}