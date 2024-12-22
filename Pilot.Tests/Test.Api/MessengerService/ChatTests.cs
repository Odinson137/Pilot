using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Api.MessengerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.MessengerService;

[Collection(nameof(SequentialCollectionDefinition))]
public class ChatTests : BaseMessengerServiceIntegrationTest
{
    public ChatTests(MessengerTestApiFactory apiFactory, MessengerTestIdentityFactory identityFactory, MessengerTestMessengerFactory messengerFactory)
        : base(apiFactory, identityFactory, messengerFactory) { }

    private static string EntityName => nameof(Chat);

    [Fact]
    public virtual async Task GetUserChatsTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<Chat>(count: count, listDepth: 0);
        foreach (var value in values)
        {
            value.AddUser(1);
        }
        await GetContext<ChatDto>().AddRangeAsync(values);
        await GetContext<ChatDto>().SaveChangesAsync();
        
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}/{Urls.UserChats}");
    
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<ChatDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    [Fact]
    public virtual async Task CreateModel_ReturnOk()
    {
        #region Arrange

        var valueModel = GenerateTestEntity.CreateEntities<Chat>(count: 1, listDepth: 0).First();

        var value = Mapper.Map<ChatDto>(valueModel);

        #endregion

        // Act
        
        var responseMessage = await ApiClient.PostAsJsonAsync($"api/{EntityName}", value);
        await Helper.Wait();

        // Assert

        Assert.True(responseMessage.IsSuccessStatusCode);
        var result = await AssertContext.Set<Chat>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();

        Assert.NotNull(result);
    }
}