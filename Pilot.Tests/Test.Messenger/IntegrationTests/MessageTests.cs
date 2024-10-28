using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class MessageTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<Message, MessageDto>(factory, identityFactory)
{
    [Fact]
    public virtual async Task GetUserChatsTest_ReturnOk()
    {
        #region Arrange

        var user = await CreateUser();

        const int count = 2;
        var chat = GenerateTestEntity.CreateEntities<Chat>(count: 1, listDepth: 0).First();
        var values = GenerateTestEntity.CreateEntities<Message>(count: 2, listDepth: 0);

        chat.AddUser(user.Id);

        chat.Messages = values;
        
        await DataContext.AddRangeAsync(chat);
        await DataContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await MessengerClient.GetAsync($"api/{nameof(Message)}/{Urls.ChatMessages}/{chat.Id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<MessageDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}