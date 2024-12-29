using System.Net.Http.Json;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Api.MessengerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.MessengerService;

[Collection(nameof(SequentialCollectionDefinition))]
public class MessageTests : BaseMessengerServiceIntegrationTest
{
    public MessageTests(MessengerTestApiFactory apiFactory, MessengerTestIdentityFactory identityFactory, MessengerTestMessengerFactory messengerFactory)
        : base(apiFactory, identityFactory, messengerFactory) { }

    private static string EntityName => nameof(Message);

    [Fact]
    public virtual async Task GetChatMessagesTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var chat = GenerateTestEntity.CreateEntities<Chat>(count: 1, listDepth: 0).First();
        var values = GenerateTestEntity.CreateEntities<Message>(count: count, listDepth: 0);
        foreach (var value in values)
        {
            value.AddUser(1);
        }
        chat.AddUser(1);
        chat.Messages = values;
        
        await GetContext<ChatDto>().AddRangeAsync(chat);
        await GetContext<ChatDto>().SaveChangesAsync();
        
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}/{Urls.ChatMessages}/{chat.Id}");
    
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<ChatDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}