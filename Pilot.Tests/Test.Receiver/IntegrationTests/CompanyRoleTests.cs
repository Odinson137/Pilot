using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class CompanyRoleTests : BaseModelReceiverIntegrationTest<CompanyRole, CompanyRoleDto>
{
    /// <inheritdoc />
    public CompanyRoleTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }
}