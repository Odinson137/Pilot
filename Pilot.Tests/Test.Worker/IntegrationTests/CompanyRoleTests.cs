using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class CompanyRoleTests : BaseModelReceiverIntegrationTest<CompanyRole, CompanyRoleDto>
{
    /// <inheritdoc />
    public CompanyRoleTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory,
        WorkerTestStorageFactory storageFactory,
        WorkerTestAuditHistoryFactory auditHistoryFactory) :
        base(workerTestWorkerFactory, identityFactory, storageFactory, auditHistoryFactory)
    {
    }
}