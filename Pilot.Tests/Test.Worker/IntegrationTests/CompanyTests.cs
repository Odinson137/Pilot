﻿using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class CompanyTests : BaseModelReceiverIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory, WorkerTestStorageFactory storageFactory) :
        base(workerTestWorkerFactory, identityFactory, storageFactory)
    {
    }
    
    // TODO потом просто сделать так, как я сделал в тестировании fileurl для всех моделей
    // protected override async ValueTask GetArrangeDop(ICollection<Company> values)
    // {
    //     foreach (var company in values)
    //     {
    //         company.LogoId = 1;
    //         company.InsideImagesId = [2];
    //     }
    // }
}