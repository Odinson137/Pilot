using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.AuditHistory.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Identity.Models;
using Test.Api.AuditHistoryService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.AuditHistoryService;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class AuditHistoryServiceIntegrationTest<T, TDto>(
    AuditHistoryTestApiFactory apiFactory,
    AuditHistoryTestAuditHistoryFactory auditHistoryFactory,
    AuditHistoryTestWorkerFactory workerFactory,
    AuditHistoryTestIdentityFactory identityFactory
    )
    : BaseAuditHistoryIntegrationTest(
        apiFactory, 
        auditHistoryFactory, 
        workerFactory,
        identityFactory
        )
    where T : BaseModel
    where TDto : BaseDto
{
    #region Aditional methods

    private async Task<User> CreateUser(bool withAuthorization = false)
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        var userContext = GetContext<UserDto>();
        await userContext.AddRangeAsync(user);
        await userContext.SaveChangesAsync();
    
        if (withAuthorization)
        {
            var token = TokenService.GenerateToken(user.Id, Role.Junior);
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return user;
    }

    private async Task<bool> ClickHouseCheckingAsync(T value)
    {
        var context = GetContext<AuditHistoryDto>();
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

        await GenerateTestEntity.FillChildren(value, GetContext<TDto>());

        var valueDto = GetMapper<TDto>().Map<TDto>(value);

        var token = TokenService.GenerateToken(user.Id, Role.Junior);
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        #endregion

        // Act
        var result = await ApiClient.PostAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext<TDto>().Set<T>().Where(c => c.CreateAt == value.CreateAt)
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

        var context = GetContext<TDto>();
        await context.AddRangeAsync(value);
        await context.SaveChangesAsync();

        var valueDto = GetMapper<TDto>().Map<TDto>(value);

        #endregion

        // Act
        var result = await ApiClient.PutAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext<TDto>().Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
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

        var context = GetContext<TDto>();
        await context.AddRangeAsync(value);
        await context.SaveChangesAsync();

        #endregion

        // Act
        var result = await ApiClient.DeleteAsync($"api/{type.Name}/{value.Id}");
        await Helper.Wait();

        // Assert

        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext<TDto>().Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.Null(content);
        
        // Assert 2 то ради чего и создавалась вся эта папка
        Assert.True(await ClickHouseCheckingAsync(value));
    }
}