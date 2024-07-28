using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Tests.IntegrationBase;
using Pilot.Tests.Receiver.Tests.IntegrationTests.Factories;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class CompanyReceiverTests : BaseModelReceiverIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyReceiverTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }

    public override void ChangeSomething(BaseDto model)
    {
        ((CompanyDto)model).Title = Guid.NewGuid().ToString();
    }
    
    public override bool CheckChangeSomething(BaseModel model, BaseDto modelDto)
    {
        return ((Company)model).Title == ((CompanyDto)modelDto).Title;
    }
    
    // [Fact]
    // public override async void CreateModel_ReturnOk()
    // {
    //     #region Arrange
    //     
    //     var companyUser = await CreateCompanyUser();
    //     
    //     var value = GenerateTestEntity.CreateEntities<CompanyDto>(count: 1, listDepth: 0).First();
    //     value.CreateAt = DateTime.Now;
    //     value.Title = Guid.NewGuid().ToString();
    //     
    //     #endregion
    //
    //     // Act
    //
    //     await PublishEndpoint.Publish(new CreateCommandMessage<CompanyDto>(value, companyUser.Id));
    //
    //     // Assert
    //     await Wait();
    //
    //     var result = await ReceiverContext.Companies.Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();
    //
    //     Assert.NotNull(result);
    //     Assert.True(result.Title == value.Title);
    // }
}