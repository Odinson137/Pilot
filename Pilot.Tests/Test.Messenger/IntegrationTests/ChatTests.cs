using System.Net.Http.Json;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Base.IntegrationBase;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class ChatTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<Chat, ChatDto>(factory, identityFactory)
{
    [Fact]
    public virtual async Task GetUserChatsTest_ReturnOk()
    {
        #region Arrange

        var user = await CreateUser();

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<Chat>(count: 2, listDepth: 0);

        foreach (var chat in values) chat.AddUser(user.Id);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await MessengerClient.GetAsync($"api/{nameof(Chat)}/{Urls.UserChats}/{user.Id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<ChatDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}