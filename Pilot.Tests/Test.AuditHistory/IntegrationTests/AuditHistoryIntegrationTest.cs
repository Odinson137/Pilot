using System.Net.Http.Json;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Test.AuditHistory.IntegrationTests.Factories;
using Test.Base.IntegrationBase;
using Xunit.Abstractions;

namespace Test.AuditHistory.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class AuditHistoryIntegrationTest(
    AuditHistoryTestAuditHistoryFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseAuditHistoryIntegrationTest(factory)
{
    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;

    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<Pilot.AuditHistory.Models.AuditHistory>(count: 2, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(Pilot.AuditHistory.Models.AuditHistory)}");

        TestOutputHelper.WriteLine(await result.Content.ReadAsStringAsync());

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<AuditHistoryDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }

    [Fact]
    public virtual async Task GetValueTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values =
            GenerateTestEntity.CreateEntities<Pilot.AuditHistory.Models.AuditHistory>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            Skip = 0,
            Take = 1,
            WhereFilter = new WhereFilter((nameof(AuditHistoryDto.EntityId), values.First().EntityId),
                (nameof(AuditHistoryDto.EntityType), ModelType.ProjectTask))
        };

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(Pilot.AuditHistory.Models.AuditHistory)}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<AuditHistoryDto>>();
        Assert.NotNull(content);
        Assert.Single(content);
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