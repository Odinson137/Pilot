using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class CompanyRoleTests : BaseModelReceiverIntegrationTest<CompanyRole, CompanyRoleDto>
{
    /// <inheritdoc />
    public CompanyRoleTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }
}