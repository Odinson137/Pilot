﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class CompanyUserTests : BaseModelReceiverIntegrationTest<CompanyUser, CompanyUserDto>
{
    /// <inheritdoc />
    public CompanyUserTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }

    [Fact]
    public override async void UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var mapper = ReceiverScope.ServiceProvider.GetRequiredService<IMapper>();
        var valueDto = mapper.Map<CompanyUserDto>(companyUser);

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