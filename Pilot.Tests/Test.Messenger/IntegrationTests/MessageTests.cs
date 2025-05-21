using System.Net.Http.Json;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.Messenger.Models;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Messenger.IntegrationTests;

public class MessageTests(
    TestIdentityFactory identityFactory,
    TestMessengerFactory messengerFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<Message, MessageDto>(testOutputHelper, ServiceName.MessengerServer,
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

        var user = (User)await CreateUser(false); // TODO мне не нравится, сделать получше

        const int count = 2;
        var chat = GenerateTestEntity.CreateEntities<Chat>(count: 1, listDepth: 0).First();
        var values = GenerateTestEntity.CreateEntities<Message>(count: 2, listDepth: 0);

        chat.AddUser(user.Id);

        chat.Messages = values;

        var dataContext = GetContext(ServiceName.MessengerServer);
        await dataContext.AddRangeAsync(chat);
        await dataContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(Message)}/{Urls.ChatMessages}/{chat.Id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<MessageDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}