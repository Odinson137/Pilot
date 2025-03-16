using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.Worker.Models;
using Pilot.Worker.Models.ModelHelpers;
using Test.Api.WorkerService.Factory;
using Test.Base.IntegrationBase;
using Xunit.Abstractions;

namespace Test.Api.WorkerService;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class WorkerTests<T, TDto> : BaseWorkerServiceIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    private readonly ITestOutputHelper _testOutputHelper;

    public WorkerTests(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory,
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory, ITestOutputHelper testOutputHelper)
        : base(apiFactory, identityFactory, workerFactory, storageFactory)
    {
        _testOutputHelper = testOutputHelper;
        AssertContext.Database.EnsureDeleted();
        AssertContext.Database.EnsureCreated();
    }

    private static string EntityName => typeof(T).Name;
    
    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
    
        await GetContext<CompanyDto>().AddRangeAsync(values);
        await GetContext<CompanyDto>().SaveChangesAsync();
    
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}");
        _testOutputHelper.WriteLine($"Status Code: {result.StatusCode}");
        _testOutputHelper.WriteLine($"Response: {await result.Content.ReadAsStringAsync()}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    [Fact]
    public virtual async Task GetValueTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 1;
    
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
    
        await GetContext<CompanyDto>().AddRangeAsync(values);
        await GetContext<CompanyDto>().SaveChangesAsync();
    
        var id = values.First().Id;
    
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}/{id}");
        _testOutputHelper.WriteLine($"Status Code: {result.StatusCode}");
        _testOutputHelper.WriteLine($"Response: {await result.Content.ReadAsStringAsync()}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<TDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    protected async Task<CompanyUser> CreateCompanyUser(bool withAuthorization = false)
    {
        var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1, listDepth: 0).First();
    
        await GetContext<TDto>().AddAsync(companyUser);
        await GetContext<TDto>().SaveChangesAsync();
    
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
        user.Id = companyUser.Id;
    
        await GetContext<UserDto>().AddRangeAsync(user);
        await GetContext<UserDto>().SaveChangesAsync();
    
        if (withAuthorization)
        {
            var token = TokenService.GenerateToken(companyUser.Id, Role.Junior);
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return companyUser;
    }
    
    [Fact]
    public async Task CreateValue_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(value, GetContext<TDto>());

        var valueDto = Mapper.Map<TDto>(value);

        var token = TokenService.GenerateToken(companyUser.Id, Role.Junior);
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
    //
    // [Fact]
    // public async Task CreateValue_AddWithFile_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var companyUser = await CreateCompanyUser(true);
    //
    //     var type = typeof(T);
    //
    //     var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //     await GenerateTestEntity.FillChildren(value, GetContext<TDto>());
    //     await GenerateTestEntity.FillImage<T, TDto>(value, GetContext<FileDto>());
    //
    //     var valueDto = Mapper.Map<TDto>(value);
    //
    //     var token = TokenService.GenerateToken(companyUser.Id, Role.User);
    //     ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     
    //     #endregion
    //
    //     // Act
    //     var result = await ApiClient.PostAsJsonAsync($"api/{type.Name}", valueDto);
    //     await Helper.Wait();
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await AssertContext.Set<T>().Where(c => c.CreateAt == value.CreateAt)
    //         .FirstOrDefaultAsync();
    //     Assert.NotNull(content);
    // }

    [Fact]
    public async Task UpdateValue_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser(true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(companyUser);

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

        await CreateCompanyUser(true);

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