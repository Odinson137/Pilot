﻿using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class ProjectTaskTests : BaseModelReceiverIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory,
        WorkerTestStorageFactory storageFactory,
        WorkerTestAuditHistoryFactory auditHistoryFactory) :
        base(workerTestWorkerFactory, identityFactory, storageFactory, auditHistoryFactory)
    {
    }
}