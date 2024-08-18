using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class TeamTests : BaseModelReceiverIntegrationTest<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(
        receiverFactory, identityFactory)
    {
    }
}