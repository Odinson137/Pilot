using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.Models.ModelHelpers;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Tests.IntegrationBase;
using Pilot.Tests.Receiver.Tests.IntegrationTests.Factories;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class CompanyUserReceiverTests : BaseModelReceiverIntegrationTest<CompanyUser, CompanyUserDto>
{
    /// <inheritdoc />
    public CompanyUserReceiverTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }

    public override void ChangeSomething(BaseDto model)
    {
        ((CompanyUserDto)model).UserName = Guid.NewGuid().ToString();
    }
    
    public override bool CheckChangeSomething(BaseModel model, BaseDto modelDto)
    {
        return ((CompanyUser)model).Name == ((CompanyUserDto)modelDto).Name;
    }
    
    [Fact]
    [TestBeforeAfter]
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
        Assert.True(result.ChangeAt != null);
    }

}