﻿using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class CompanyUserTests : BaseModelReceiverIntegrationTest<CompanyUser, CompanyUserDto>
{
    /// <inheritdoc />
    public CompanyUserTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory,
        WorkerTestStorageFactory storageFactory,
            WorkerTestAuditHistoryFactory auditHistoryFactory) :
        base(workerTestWorkerFactory, identityFactory, storageFactory, auditHistoryFactory)
    {
    }

    [Fact]
    public override async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var valueDto = WorkerMapper.Map<CompanyUserDto>(companyUser);

        #endregion

        // Act

        await PublishEndpoint.Publish(new UpdateCommandMessage<CompanyUserDto>(valueDto, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<CompanyUser>().Where(c => c.Id == companyUser.Id)
            .FirstOrDefaultAsync();

        Assert.NotNull(result);
    }
}