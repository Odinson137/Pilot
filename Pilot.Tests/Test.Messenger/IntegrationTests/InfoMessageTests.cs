using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Messenger.IntegrationTests;

public class InfoMessageTests(
    TestIdentityFactory identityFactory,
    TestMessengerFactory messengerFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<InfoMessage, InfoMessageDto>(testOutputHelper, ServiceName.MessengerServer,
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
    private IRedisService RedisService => identityFactory.Services.GetRequiredService<IRedisService>();
    
    [Fact(Skip = "У InfoMessage не будет возможности изменять сообщение и удалять их, только создание")]
    public override Task UpdateModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }

    [Fact(Skip = "У InfoMessage не будет возможности изменять сообщение и удалять их, только создание")]
    public override Task DeleteModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }

    [Fact(Skip = "У InfoMessage не будет возможности изменять сообщение и удалять их, только создание")]
    public override async Task CreateModelTest_ReturnOk()
    {
        #region Arrange

        var valueModel = GenerateTestEntity.CreateEntities<InfoMessageDto>(count: 1, listDepth: 0).First();

        var user = await CreateUser();

        #endregion

        // Act

        await Publisher.Publish(new CreateCommandMessage<InfoMessageDto>(valueModel, user.Id));
        await Helper.Wait();

        // Assert

        var result = await RedisService.GetQueueValuesAsync<InfoMessageDto>(user.Id.ToString());

        Assert.NotNull(result);
    }
}