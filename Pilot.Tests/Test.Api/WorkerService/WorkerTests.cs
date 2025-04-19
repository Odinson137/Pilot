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
        var assertContext = GetContext<TDto>();
        assertContext.Database.EnsureDeleted();
        assertContext.Database.EnsureCreated();
    }

    private static string EntityName => typeof(T).Name;
    
    private static void FillUser(ICollection<T> values)
    {
        foreach (var value in values)
            FillUser(value);
    }

    private static void FillUser(T value)
    {
        if (value is IAddCompanyUser addCompanyUser)
            addCompanyUser.AddCompanyUser(new CompanyUser {Company = new Company {Title = Guid.NewGuid().ToString()}});
    }

    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        FillUser(values);
        
        var context = GetContext<TDto>();
        await context.AddRangeAsync(values);
        await context.SaveChangesAsync();
    
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
        FillUser(values);

        var context = GetContext<TDto>();
        await context.AddRangeAsync(values);
        await context.SaveChangesAsync();
    
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

    protected async Task<CompanyUser> CreateCompanyUser(DbContext context, bool withAuthorization = false)
    {
        var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1, listDepth: 0).First();

        await context.AddAsync(companyUser);
        await context.SaveChangesAsync();
    
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
        user.Id = companyUser.Id;

        var identityContext = GetContext<UserDto>();
        await identityContext.AddRangeAsync(user);
        await identityContext.SaveChangesAsync();
    
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

        var dbContext = GetContext<TDto>();
        var companyUser = await CreateCompanyUser(dbContext, true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(value, dbContext);

        var valueDto = Mapper.Map<TDto>(value);

        var token = TokenService.GenerateToken(companyUser.Id, Role.Junior);
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
    }

    [Fact]
    public async Task UpdateValue_ReturnOk()
    {
        #region Arrange

        var dbContext = GetContext<TDto>();
        var companyUser = await CreateCompanyUser(dbContext, true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(companyUser);

        await dbContext.AddRangeAsync(value);
        await dbContext.SaveChangesAsync();

        var valueDto = Mapper.Map<TDto>(value);

        #endregion

        // Act
        var result = await ApiClient.PutAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext<TDto>().Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.NotNull(content);
        Assert.NotNull(content.ChangeAt);
    }

    [Fact]
    public async Task DeleteValue_ReturnOk()
    {
        #region Arrange

        var dbContext = GetContext<TDto>();
        await CreateCompanyUser(dbContext, true);

        var type = typeof(T);

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await dbContext.AddRangeAsync(value);
        await dbContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await ApiClient.DeleteAsync($"api/{type.Name}/{value.Id}");
        await Helper.Wait();

        // Assert

        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext<TDto>().Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
        Assert.Null(content);
    }
}