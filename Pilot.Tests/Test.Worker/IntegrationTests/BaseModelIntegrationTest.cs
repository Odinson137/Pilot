﻿using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Pilot.Worker.Models.ModelHelpers;
using Serialize.Linq.Serializers;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;
using JsonSerializer = Serialize.Linq.Serializers.JsonSerializer;

namespace Test.Worker.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelReceiverIntegrationTest<T, TDto> : BaseReceiverIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BaseModelReceiverIntegrationTest(WorkerTestWorkerFactory workerTestWorkerFactory,
        WorkerTestIdentityFactory identityFactory, WorkerTestStorageFactory storageFactory,
        WorkerTestAuditHistoryFactory auditHistoryFactory) : base(
        workerTestWorkerFactory, identityFactory, storageFactory, auditHistoryFactory)
    {
        AssertReceiverContext.Database.EnsureDeleted();
        AssertReceiverContext.Database.EnsureCreated();
    }

    public string EntityName => typeof(T).Name;

    protected async Task<CompanyUser> CreateCompanyUser()
    {
        var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1, listDepth: 0).First();

        await WorkerContext.AddAsync(companyUser);
        await WorkerContext.SaveChangesAsync();

        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
        user.Id = companyUser.Id;

        await IdentityContext.AddRangeAsync(user);
        await IdentityContext.SaveChangesAsync();

        return companyUser;
    }

    [Fact]
    public virtual async Task GetAllValuesTest_FilterWithIds_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await WorkerContext.AddRangeAsync(values);
        await WorkerContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            Ids = values.Select(c => c.Id).ToList(),
        };

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }

    [Fact]
    public virtual async Task GetAllValuesTest_FilterWithWhereFilter_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await WorkerContext.AddRangeAsync(values);
        await WorkerContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            WhereFilter = new WhereFilter((nameof(BaseId.Id), values.Select(c => c.Id).First()))
        };

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count == 1);
    }

    [Fact]
    public virtual async Task GetAllValuesTest_WithSelectQuery_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await WorkerContext.AddRangeAsync(values);
        await WorkerContext.SaveChangesAsync();

        Expression<Func<T, object>> projection = c => new { c.Id, c.CreateAt };
        var filter = new BaseFilter
        {
            SelectQuery = new ExpressionSerializer(new JsonSerializer()).SerializeText(projection)
        };

        #endregion

        // Act
        var content = new StringContent(filter.ToJson(), Encoding.UTF8, "application/json");
        var result = await Client.PostAsync($"api/{EntityName}", content);

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var resultContent = await result.Content.ReadFromJsonAsync<ICollection<BaseDto>>();
        Assert.NotNull(resultContent);
        Assert.True(resultContent.Count >= count);
    }

    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await WorkerContext.AddRangeAsync(values);
        await WorkerContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}");

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

        const int count = 1;

        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await WorkerContext.AddRangeAsync(values);
        await WorkerContext.SaveChangesAsync();
        var result23 = await AssertReceiverContext.Set<T>().ToListAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}/{id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<TDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    // [Fact]
    // public virtual async void GetAllValuesWithFileTest_ReturnOk()
    // {
    //     #region Arrange
    //
    //     const int count = 2;
    //     var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
    //     await GenerateTestEntity.FillImage<T, TDto>(values, StorageContext);
    //
    //     await WorkerContext.AddRangeAsync(values);
    //     await WorkerContext.SaveChangesAsync();
    //
    //     #endregion
    //
    //     // Act
    //     var result = await Client.GetAsync($"api/{EntityName}");
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
    //     Assert.NotNull(content);
    //     Assert.True(content.Count >= count);
    // }
    //
    // [Fact]
    // public virtual async void GetValueWithFileTest_ReturnOk()
    // {
    //     #region Arrange
    //
    //     const int count = 1;
    //
    //     var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
    //     await GenerateTestEntity.FillImage<T, TDto>(values, StorageContext);
    //
    //     await WorkerContext.AddRangeAsync(values);
    //     await WorkerContext.SaveChangesAsync();
    //
    //     var id = values.First().Id;
    //
    //     #endregion
    //
    //     // Act
    //     var result = await Client.GetAsync($"api/{EntityName}/{id}");
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await result.Content.ReadFromJsonAsync<TDto>();
    //     Assert.NotNull(content);
    //     Assert.Equal(id, content.Id);
    // }

    [Fact]
    public virtual async Task CreateModel_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var valueModel = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(valueModel, WorkerContext);

        var value = WorkerMapper.Map<TDto>(valueModel);

        #endregion

        // Act

        await PublishEndpoint.Publish(new CreateCommandMessage<TDto>(value, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<T>().Where(c => c.CreateAt == value.CreateAt)
            .FirstOrDefaultAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public virtual async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(companyUser);

        await WorkerContext.AddAsync(value);
        await WorkerContext.SaveChangesAsync();

        var valueDto = WorkerMapper.Map<TDto>(value);

        #endregion

        // Act

        await PublishEndpoint.Publish(new UpdateCommandMessage<TDto>(valueDto, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(result.ChangeAt);
    }

    [Fact]
    public virtual async Task DeleteModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await WorkerContext.AddAsync(value);
        await WorkerContext.SaveChangesAsync();

        #endregion

        // Act

        await PublishEndpoint.Publish(new DeleteCommandMessage<TDto>(value.Id, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.Null(result);
    }
}