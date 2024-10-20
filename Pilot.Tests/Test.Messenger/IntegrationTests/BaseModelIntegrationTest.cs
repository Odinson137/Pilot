using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelTest<T, TDto> : BaseMessageIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BaseModelTest(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory) : base(factory, identityFactory)
    {
    }
    
    protected async Task<User> CreateUser()
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        await IdentityDataContext.AddRangeAsync(user);
        await IdentityDataContext.SaveChangesAsync();

        return user;
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
        var result = await MessengerClient.GetAsync($"api/{typeof(T).Name}");

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
        var result = await MessengerClient.GetAsync($"api/{typeof(T).Name}/{id}");

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
    
        await GenerateTestEntity.FillChildren(valueModel, DataContext);
    
        var value = MessengerMapper.Map<TDto>(valueModel);

        var user = await CreateUser();
        
        #endregion
    
        // Act
    
        await PublishEndpoint.Publish(new CreateCommandMessage<TDto>(value, user.Id));
        await Helper.Wait();
    
        // Assert
    
        var result = await AssertContext.Set<T>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();
    
        Assert.NotNull(result);
    }
    
    [Fact]
    public virtual async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange
    
        var user = await CreateUser();
    
        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    
        await DataContext.AddAsync(value);
        await DataContext.SaveChangesAsync();
    
        var valueDto = MessengerMapper.Map<TDto>(value);
    
        #endregion
    
        // Act
    
        await PublishEndpoint.Publish(new UpdateCommandMessage<TDto>(valueDto, user.Id));
        await Helper.Wait();
    
        // Assert
    
        var result = await AssertContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    
        Assert.NotNull(result);
        Assert.NotNull(result.ChangeAt);
    }
    
    [Fact]
    public virtual async Task DeleteModelTest_ReturnOk()
    {
        #region Arrange
    
        var user = await CreateUser();
    
        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    
        await DataContext.AddAsync(value);
        await DataContext.SaveChangesAsync();
    
        #endregion
    
        // Act
    
        await PublishEndpoint.Publish(new DeleteCommandMessage<TDto>(value.Id, user.Id));
        await Helper.Wait();
        
        // Assert
        
        var result = await AssertContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    
        Assert.Null(result);
    }
}