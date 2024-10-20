using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class MessageTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<Message, MessageDto>(factory, identityFactory)
{
    // [Fact]
    // public override async Task CreateModel_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var valueModel = GenerateTestEntity.CreateEntities<Message>(count: 1, listDepth: 0).First();
    //
    //     await GenerateTestEntity.FillChildren(valueModel, DataContext);
    //
    //     var value = MessengerMapper.Map<MessageDto>(valueModel);
    //
    //     var user = await CreateUser();
    //     
    //     #endregion
    //
    //     // Act
    //
    //     await PublishEndpoint.Publish(new CreateCommandMessage<MessageDto>(value, user.Id));
    //     await Helper.Wait();
    //
    //     // Assert
    //
    //     var result = await AssertContext.Set<Message>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();
    //
    //     Assert.NotNull(result);
    // }
}