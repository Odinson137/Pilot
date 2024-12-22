using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.Worker.Models;
using Pilot.Worker.Models.ModelHelpers;
using Test.Api.CapabilityService.Factory;
using Test.Base.IntegrationBase;
using IAddCompanyUser = Pilot.Capability.Models.ModelHelpers.IAddCompanyUser;

namespace Test.Api.CapabilityService;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class CapabilityTests<T, TDto> : BaseCapabilityServiceIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public CapabilityTests(CapabilityTestApiFactory apiFactory, CapabilityTestIdentityFactory identityFactory, CapabilityTestCapabilityFactory capabilityFactory, CapabilityTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, capabilityFactory, storageFactory) { }

    protected static string EntityName => typeof(T).Name;

    [Fact]
    public virtual async Task GetAllValuesWithFileTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        await GenerateTestEntity.FillImage<T, TDto>(values, GetContext<FileDto>());
    
        await GetContext<SkillDto>().AddRangeAsync(values);
        await GetContext<SkillDto>().SaveChangesAsync();
    
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
    
        await GetContext<SkillDto>().AddRangeAsync(values);
        await GetContext<SkillDto>().SaveChangesAsync();
    
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
            var token = TokenService.GenerateToken(user.Id, Role.User);
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

        var token = TokenService.GenerateToken(user.Id, Role.User);
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

        if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(user.Id);

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
        Assert.NotNull(content.ChangeAt);
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