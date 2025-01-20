using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Test.Api.BackgroundJobService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.BackgroundJobService;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BackgroundJobTests<T, TDto> : BaseBackgroundJobServiceIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BackgroundJobTests(
        BackgroundJobTestApiFactory apiFactory, 
        BackgroundJobTestIdentityFactory identityFactory, 
        BackgroundJobTestBackgroundJobFactory capabilityFactory,
        BackgroundJobTestStorageFactory storageFactory
        )
        : base(apiFactory, identityFactory, capabilityFactory, storageFactory) {
        AssertContext.Database.EnsureDeleted();
        AssertContext.Database.EnsureCreated();
    }

    protected static string EntityName => typeof(T).Name;

    [Fact]
    public virtual async Task GetAllValuesWithFileTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        await GenerateTestEntity.FillImage<T, TDto>(values, GetContext<FileDto>());
    
        await GetContext<TDto>().AddRangeAsync(values);
        await GetContext<TDto>().SaveChangesAsync();
    
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}");
    
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    [Fact]
    public virtual async Task GetValueWithFileTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 1;
    
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        await GenerateTestEntity.FillImage<T, TDto>(values, GetContext<FileDto>());
    
        await GetContext<TDto>().AddRangeAsync(values);
        await GetContext<TDto>().SaveChangesAsync();
    
        var id = values.First().Id;
    
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}/{id}");
    
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<TDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    protected async Task<User> CreateUser(bool withAuthorization = false)
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
    
        await GetContext<UserDto>().AddRangeAsync(user);
        await GetContext<UserDto>().SaveChangesAsync();
    
        if (withAuthorization)
        {
            var token = TokenService.GenerateToken(user.Id, Role.Junior);
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return user;
    }
    
    [Fact]
    public async Task CreateValue_ReturnOk()
    {
        #region Arrange

        var user = await CreateUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(value, GetContext<TDto>());

        var valueDto = Mapper.Map<TDto>(value);

        var token = TokenService.GenerateToken(user.Id, Role.Junior);
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        #endregion

        // Act
        var result = await ApiClient.PostAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await AssertContext.Set<T>().Where(c => c.CreateAt == value.CreateAt)
            .FirstOrDefaultAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task UpdateValue_ReturnOk()
    {
        #region Arrange

        var user = await CreateUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GetContext<TDto>().AddRangeAsync(value);
        await GetContext<TDto>().SaveChangesAsync();

        var valueDto = Mapper.Map<TDto>(value);

        #endregion

        // Act
        var result = await ApiClient.PutAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await AssertContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.NotNull(content);
        Assert.NotNull((object?)content.ChangeAt);
    }

    [Fact]
    public async Task DeleteValue_ReturnOk()
    {
        #region Arrange

        await CreateUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GetContext<TDto>().AddRangeAsync(value);
        await GetContext<TDto>().SaveChangesAsync();

        #endregion

        // Act
        var result = await ApiClient.DeleteAsync($"api/{type.Name}/{value.Id}");
        await Helper.Wait();

        // Assert

        Assert.True(result.IsSuccessStatusCode);
        var content = await AssertContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.Null(content);
    }
}