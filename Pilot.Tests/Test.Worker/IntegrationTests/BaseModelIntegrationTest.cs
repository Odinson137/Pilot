using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Pilot.Worker.Models.ModelHelpers;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelReceiverIntegrationTest<T, TDto> : BaseReceiverIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BaseModelReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory,
        ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }

    public string EntityName => typeof(T).Name;

    protected virtual async Task<User> CreateUser()
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        await IdentityContext.AddRangeAsync(user);
        await IdentityContext.SaveChangesAsync();

        return user;
    }

    protected async Task<CompanyUser> CreateCompanyUser()
    {
        var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1, listDepth: 0).First();

        await ReceiverContext.AddAsync(companyUser);
        await ReceiverContext.SaveChangesAsync();

        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
        user.Id = companyUser.Id;

        await IdentityContext.AddRangeAsync(user);
        await IdentityContext.SaveChangesAsync();

        return companyUser;
    }

    [Fact]
    public virtual async void GetAllValuesTest_FilterWithIdsReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            Ids = values.Select(c => c.Id).ToList(),
        };
        
        #endregion

        // Act
        var result = await ReceiverClient.GetAsync($"api/{EntityName}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    // protected virtual async ValueTask GetArrangeDop(ICollection<T> values) {}
    
    [Fact]
    public virtual async void GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);

        // await GetArrangeDop(values);
        
        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await ReceiverClient.GetAsync($"api/{EntityName}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    [Fact]
    public virtual async void GetValue_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        // await GetArrangeDop(values);

        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await ReceiverClient.GetAsync($"api/{EntityName}/{id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<TDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    [Fact]
    public virtual async void CreateModel_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var valueModel = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(valueModel, ReceiverContext);

        var value = ReceiverMapper.Map<TDto>(valueModel);

        #endregion

        // Act

        await PublishEndpoint.Publish(new CreateCommandMessage<TDto>(value, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<T>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public virtual async void UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(companyUser);

        await ReceiverContext.AddAsync(value);
        await ReceiverContext.SaveChangesAsync();

        var valueDto = ReceiverMapper.Map<TDto>(value);

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
    public virtual async void DeleteModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await ReceiverContext.AddAsync(value);
        await ReceiverContext.SaveChangesAsync();

        #endregion

        // Act

        await PublishEndpoint.Publish(new DeleteCommandMessage<TDto>(value.Id, companyUser.Id));
        await Helper.Wait();
        
        // Assert
        
        var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.Null(result);
    }
}