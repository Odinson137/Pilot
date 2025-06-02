using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.AuditHistory.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;
using Pilot.Identity.Models;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;

namespace Test.Api.AuditHistoryService;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class AuditHistoryServiceIntegrationTest<T, TDto>(
    ServiceName serviceName,
    TestApiFactory apiFactory,
    TestAuditHistoryFactory auditHistoryFactory,
    TestWorkerFactory workerFactory,
    TestIdentityFactory identityFactory)
    : BaseIntegrationTest(apiConfiguration: new ServiceTestConfiguration
            {
                ServiceName = ServiceName.ApiServer,
                ServiceProvider = apiFactory.Services,
                HttpClient = apiFactory.CreateClient(),
            },
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
                    ServiceName = ServiceName.WorkerServer,
                    ServiceProvider = workerFactory.Services,
                    DbContextType = typeof(Pilot.Worker.Data.DataContext),
                    HttpClient = workerFactory.CreateClient(),
                    IsMainService = true
                },
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.AuditHistoryService,
                    DbContextType = typeof(Pilot.AuditHistory.Data.ClickHouseContext),
                    ServiceProvider = auditHistoryFactory.Services,
                    HttpClient = auditHistoryFactory.CreateClient(),
                }
            ]),
        IClassFixture<TestApiFactory>,
        IClassFixture<TestIdentityFactory>,
        IClassFixture<TestAuditHistoryFactory>,
        // IClassFixture<TestMessengerFactory>,
        IClassFixture<TestWorkerFactory> where T : BaseModel where TDto : BaseDto
{ 
    #region Aditional methods

    private async Task<User> CreateUser(bool withAuthorization = false)
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        var userContext = GetContext(ServiceName.IdentityServer);
        await userContext.AddRangeAsync(user);
        await userContext.SaveChangesAsync();
    
        if (withAuthorization)
        {
            var token = TokenService.GenerateToken(user.Id, Role.Junior);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return user;
    }
    
    private async Task<bool> ClickHouseCheckingAsync(T value)
    {
        var context = GetContext(ServiceName.AuditHistoryService);
        var type = PilotEnumExtensions.GetModelEnumDtoValue<TDto>();
        return await context.Set<AuditHistory>().AnyAsync(c => c.EntityId == value.Id && c.EntityType == type);
    }

    #endregion
    
    [Fact(Skip = "Test skipped in base class. Override to enable.")]
    protected virtual async Task Consume_CreateValue_ReturnOk()
    {
        #region Arrange

        var user = await CreateUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(value, GetContext(serviceName));

        var valueDto = Mapper.Map<TDto>(value);

        var token = TokenService.GenerateToken(user.Id, Role.Junior);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        #endregion

        // Act
        var result = await Client.PostAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext(serviceName).Set<T>().Where(c => c.CreateAt == value.CreateAt)
            .FirstOrDefaultAsync();
        Assert.NotNull(content);

        // Assert 2 то ради чего и создавалась вся эта папка
        Assert.True(await ClickHouseCheckingAsync(content));
    }

    [Fact(Skip = "Test skipped in base class. Override to enable.")]
    public virtual async Task Consume_UpdateValue_ReturnOk()
    {
        #region Arrange

        await CreateUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        var context = GetContext(serviceName);
        await context.AddRangeAsync(value);
        await context.SaveChangesAsync();

        var valueDto = Mapper.Map<TDto>(value);

        #endregion

        // Act
        var result = await Client.PutAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext(serviceName).Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.NotNull(content);
        Assert.NotNull(content.ChangeAt);
        
        // Assert 2 то ради чего и создавалась вся эта папка
        Assert.True(await ClickHouseCheckingAsync(content));
    }

    [Fact(Skip = "Test skipped in base class. Override to enable.")]
    public virtual async Task Consume_DeleteValue_ReturnOk()
    {
        #region Arrange

        await CreateUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        var context = GetContext(serviceName);
        await context.AddRangeAsync(value);
        await context.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.DeleteAsync($"api/{type.Name}/{value.Id}");
        await Helper.Wait();

        // Assert

        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext(serviceName).Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.Null(content);
        
        // Assert 2 то ради чего и создавалась вся эта папка
        Assert.True(await ClickHouseCheckingAsync(value));
    }
}