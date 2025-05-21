using System.Net.Http.Json;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.Messenger.Models;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Test.Messenger.IntegrationTests.Factories;
using Xunit.Abstractions;

namespace Test.Messenger.IntegrationTests;

public class ChatTests(
    TestIdentityFactory identityFactory,
    TestMessengerFactory messengerFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<Chat, ChatDto>(testOutputHelper, ServiceName.MessengerServer,
            configurations:
            [
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.IdentityServer,
                    ServiceProvider = identityFactory.Services,
                    DbContextType = typeof(Pilot.Identity.Data.DataContext),
                    HttpClient = identityFactory.CreateClient()
                },
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.MessengerServer,
                    ServiceProvider = messengerFactory.Services,
                    DbContextType = typeof(Pilot.Messenger.Data.DataContext),
                    HttpClient = messengerFactory.CreateClient(),
                    IsMainService = true
                },
            ]),
        IClassFixture<TestIdentityFactory>,
        IClassFixture<TestMessengerFactory>
{
    [Fact]
    public virtual async Task GetUserChatsTest_ReturnOk()
    {
        #region Arrange

        var user = (User)await CreateUser(false);

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<Chat>(count: 2, listDepth: 0);

        foreach (var chat in values) chat.AddUser(user.Id);

        var dataContext = GetContext(ServiceName.MessengerServer);
        await dataContext.AddRangeAsync(values);
        await dataContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(Chat)}/{Urls.UserChats}/{user.Id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<ChatDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}