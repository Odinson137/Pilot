using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Identity.Models;
using Pilot.Receiver.Models;
using Pilot.Receiver.Models.ModelHelpers;
using Test.Base.IntegrationBase;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelReceiverIntegrationTest<T, TDto> : BaseReceiverIntegrationTest
    where T : BaseModel where TDto : BaseDto
{
    public BaseModelReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory,
        ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
    }
    // public readonly User Admin;

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
        companyUser.UserName = Guid.NewGuid().ToString();

        await ReceiverContext.AddAsync(companyUser);
        await ReceiverContext.SaveChangesAsync();

        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
        user.Id = companyUser.Id;

        await IdentityContext.AddRangeAsync(user);
        await IdentityContext.SaveChangesAsync();

        return companyUser;
    }

    // Маленькое ухищрение, которое позволяет ожидать пока консюмер обработает сообщение из очереди,
    // А ещё нормально использовать debug, имея в запасе i кликов в нём
    public async Task Wait()
    {
        for (var i = 0; i < 40; i++) await Task.Delay(100);
    }

    [Fact]
    public virtual async void GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: 2, listDepth: 0);

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

        // Assert
        await Wait();

        var result = await ReceiverContext.Set<T>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();

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

        // Assert
        await Wait();

        var result = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
    }
}