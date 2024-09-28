using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class CompanyRoleTests : BaseModelIntegrationTest<CompanyRole, CompanyRoleDto>
{
    /// <inheritdoc />
    public CompanyRoleTests(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, receiverFactory, identityFactory)
    {
    }
}