using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.AuditHistoryService.Factory;

namespace Test.Api.AuditHistoryService;

public class ProjectTaskTest(
    AuditHistoryTestApiFactory apiFactory,
    AuditHistoryTestAuditHistoryFactory auditHistoryFactory,
    AuditHistoryTestWorkerFactory workerFactory,
    AuditHistoryTestIdentityFactory identityFactory
    )
    : AuditHistoryServiceIntegrationTest<ProjectTask, ProjectTaskDto>(apiFactory, auditHistoryFactory, workerFactory, identityFactory)
{
    [Fact]
    public override Task Consume_UpdateValue_ReturnOk()
    {
        return base.Consume_UpdateValue_ReturnOk();
    }
}