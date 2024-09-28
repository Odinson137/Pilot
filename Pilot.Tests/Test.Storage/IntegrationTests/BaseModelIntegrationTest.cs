using System.Net.Http.Json;
using Pilot.Contracts.Base;
using Test.Base.IntegrationBase;
using Test.Storage.IntegrationTests.Factories;

namespace Test.Storage.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelReceiverIntegrationTest<T, TDto> : BaseStorageIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BaseModelReceiverIntegrationTest(FileTestStorageFactory factory) : base(factory)
    {
    }
        
    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: 2, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        #endregion
        
        // Act
        var result = await CapabilityClient.GetAsync($"api/{typeof(T).Name}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }

    [Fact]
    public virtual async Task GetValue_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await CapabilityClient.GetAsync($"api/{typeof(T).Name}/{id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<TDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    // [Fact]
    // public virtual async void CreateModel_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var valueModel = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //
    //     await GenerateTestEntity.FillChildren(valueModel, DataContext);
    //
    //     var value = ReceiverMapper.Map<TDto>(valueModel);
    //
    //     #endregion
    //
    //     // Act
    //
    //     await PublishEndpoint.Publish(new CreateCommandMessage<TDto>(value, companyUser.Id));
    //     await Helper.Wait();
    //
    //     // Assert
    //
    //     var result = await ReceiverContext.Set<T>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();
    //
    //     Assert.NotNull(result);
    // }
    //
    // [Fact]
    // public virtual async void UpdateModelTest_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var companyUser = await CreateCompanyUser();
    //
    //     var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //
    //     if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(companyUser);
    //
    //     await ReceiverContext.AddAsync(value);
    //     await ReceiverContext.SaveChangesAsync();
    //
    //     var valueDto = ReceiverMapper.Map<TDto>(value);
    //
    //     #endregion
    //
    //     // Act
    //
    //     await PublishEndpoint.Publish(new UpdateCommandMessage<TDto>(valueDto, companyUser.Id));
    //     await Helper.Wait();
    //
    //     // Assert
    //
    //     var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    //
    //     Assert.NotNull(result);
    // }
    //
    // [Fact]
    // public virtual async void DeleteModelTest_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var companyUser = await CreateCompanyUser();
    //
    //     var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //
    //     await ReceiverContext.AddAsync(value);
    //     await ReceiverContext.SaveChangesAsync();
    //
    //     #endregion
    //
    //     // Act
    //
    //     await PublishEndpoint.Publish(new DeleteCommandMessage<TDto>(value.Id, companyUser.Id));
    //     await Helper.Wait();
    //     
    //     // Assert
    //     
    //     var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    //
    //     Assert.Null(result);
    // }
}