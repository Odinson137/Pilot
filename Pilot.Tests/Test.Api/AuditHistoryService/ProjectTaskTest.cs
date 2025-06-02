using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase.Factories;

namespace Test.Api.AuditHistoryService;

public class ProjectTaskTest(
    TestApiFactory apiFactory,
    TestAuditHistoryFactory auditHistoryFactory,
    TestWorkerFactory workerFactory,
    TestIdentityFactory identityFactory
)
    : AuditHistoryServiceIntegrationTest<ProjectTask, ProjectTaskDto>(ServiceName.WorkerServer, apiFactory, auditHistoryFactory, workerFactory,
        identityFactory)
{
    [Fact]
    public override Task Consume_UpdateValue_ReturnOk()
    {
        return base.Consume_UpdateValue_ReturnOk();
    }
}