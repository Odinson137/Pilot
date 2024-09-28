using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class HistoryActionTests : BaseModelIntegrationTest<HistoryAction, HistoryActionDto>
{
    /// <inheritdoc />
    public HistoryActionTests(ApiTestApiFactory apiFactory, ApiTestWorkerFactory workerFactory,
        ApiTestIdentityFactory identityFactory)
        : base(apiFactory, workerFactory, identityFactory)
    {
    }
}