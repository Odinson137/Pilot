using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelReceiverIntegrationTest<T, TDto> : BaseCapabilityIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BaseModelReceiverIntegrationTest(CapabilityTestCapabilityFactory factory) : base(factory)
    {
        AssertContext.Database.EnsureDeleted();
        AssertContext.Database.EnsureCreated();
    }

    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: 2, listDepth: 0);

        var dataContext = AssertReceiverContext;
        await dataContext.AddRangeAsync(values);
        await dataContext.SaveChangesAsync();

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

        var dataContext = AssertReceiverContext;
        await dataContext.AddRangeAsync(values);
        await dataContext.SaveChangesAsync();

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

    [Fact]
    public virtual async Task CreateModel_ReturnOk()
    {
        #region Arrange

        var valueModel = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(valueModel, AssertReceiverContext);

        var value = ReceiverMapper.Map<TDto>(valueModel);

        #endregion

        // Act

        await PublishEndpoint.Publish(new CreateCommandMessage<TDto>(value, 1));
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

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        var workerContext = AssertReceiverContext;
        await workerContext.AddAsync(value);
        await workerContext.SaveChangesAsync();

        var valueDto = ReceiverMapper.Map<TDto>(value);

        #endregion

        // Act

        await PublishEndpoint.Publish(new UpdateCommandMessage<TDto>(valueDto, 1));
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

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        var workerContext = AssertReceiverContext;
        await workerContext.AddAsync(value);
        await workerContext.SaveChangesAsync();

        #endregion

        // Act

        await PublishEndpoint.Publish(new DeleteCommandMessage<TDto>(value.Id, 1));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.Null(result);
    }
}