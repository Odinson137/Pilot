﻿using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Api.WorkerService.Factory;

namespace Test.Api.WorkerService;

public class HistoryActionTests : WorkerTests<HistoryAction, HistoryActionDto>
{
    /// <inheritdoc />
    public HistoryActionTests(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory, 
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, workerFactory, storageFactory)
    {
    }
}