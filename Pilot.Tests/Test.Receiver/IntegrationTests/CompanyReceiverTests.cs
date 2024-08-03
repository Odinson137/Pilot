using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class CompanyReceiverTests : BaseModelReceiverIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyReceiverTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }

    [Fact]
    public override async void CreateModel_ReturnOk()
    {
        #region Arrange
        
        var companyUser = await CreateCompanyUser();
        
        var value = GenerateTestEntity.CreateEntities<CompanyDto>(count: 1, listDepth: 0).First();
        value.CreateAt = DateTime.Now;
        value.Title = Guid.NewGuid().ToString();
        
        #endregion
    
        // Act
    
        await PublishEndpoint.Publish(new CreateCommandMessage<CompanyDto>(value, companyUser.Id));
    
        // Assert
        await Wait();
    
        var result = await ReceiverContext.Companies.Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();
    
        Assert.NotNull(result);
        Assert.True(result.Title == value.Title);
    }
}