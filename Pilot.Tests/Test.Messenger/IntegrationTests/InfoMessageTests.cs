using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class InfoMessageTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<InfoMessage, InfoMessageDto>(factory, identityFactory)
{
    // У InfoMessage не будет возможности изменять сообщение и удалять их, только создание
    public override Task UpdateModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }
    
    public override Task DeleteModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }
    
    [Fact]
    public override async Task CreateModel_ReturnOk()
    {
        #region Arrange
    
        var valueModel = GenerateTestEntity.CreateEntities<InfoMessageDto>(count: 1, listDepth: 0).First();
    
        var user = await CreateUser();
        
        #endregion
    
        // Act
    
        await PublishEndpoint.Publish(new CreateCommandMessage<InfoMessageDto>(valueModel, user.Id));
        await Helper.Wait();
    
        // Assert
    
        var result = await RedisService.GetQueueValuesAsync<InfoMessageDto>(user.Id.ToString());
    
        Assert.NotNull(result);
    }
}