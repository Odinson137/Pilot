using System.Net.Http.Json;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.WorkerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.WorkerService;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class WorkerTests<T, TDto> : BaseWorkerServiceIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public WorkerTests(WorkerTestApiFactory apiFactory, WorkerTestIdentityFactory identityFactory, WorkerTestWorkerFactory workerFactory, WorkerTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, workerFactory, storageFactory) { }

    private static string EntityName => typeof(T).Name;

    [Fact]
    public virtual async void GetAllValuesWithFileTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        await GenerateTestEntity.FillImage<T, TDto>(values, GetContext<FileDto>());
    
        await GetContext<CompanyDto>().AddRangeAsync(values);
        await GetContext<CompanyDto>().SaveChangesAsync();
    
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
    public virtual async void GetValueWithFileTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 1;
    
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        await GenerateTestEntity.FillImage<T, TDto>(values, GetContext<FileDto>());
    
        await GetContext<CompanyDto>().AddRangeAsync(values);
        await GetContext<CompanyDto>().SaveChangesAsync();
    
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
}