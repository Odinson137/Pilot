using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class TeamTests : BaseModelReceiverIntegrationTest<Team, TeamDto>
{
    /// <inheritdoc />
    public TeamTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(
        receiverFactory, identityFactory)
    {
    }
}