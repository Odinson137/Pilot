using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Receiver.IntegrationTests.Factories;
using Test.Receiver.IntegrationTests.TestSettings;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class CompanyUserReceiverTests : BaseModelReceiverIntegrationTest<CompanyUser, CompanyUserDto>
{
    /// <inheritdoc />
    public CompanyUserReceiverTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
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

        // Assert
        await Wait();
        
        var result = await AssertReceiverContext.Set<CompanyUser>().Where(c => c.Id == companyUser.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
    }

}