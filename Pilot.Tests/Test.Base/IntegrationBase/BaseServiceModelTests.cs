﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Pilot.Worker.Models.ModelHelpers;
using Xunit.Abstractions;

namespace Test.Base.IntegrationBase;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseServiceModelTests<T, TDto> : BaseIntegrationTest where T : BaseModel where TDto : BaseDto
{
    private readonly ServiceName _serviceName;
    private readonly ITestOutputHelper _testOutputHelper;

    public BaseServiceModelTests(
        ITestOutputHelper testOutputHelper, ServiceName serviceName,
        ServiceTestConfiguration? apiConfiguration = null,
        IEnumerable<ServiceTestConfiguration>? configurations = null) : base(apiConfiguration, configurations)
    {
        _testOutputHelper = testOutputHelper;
        _serviceName = serviceName;
    }

    protected static string EntityName => typeof(T).Name;

    private static void FillUser(ICollection<T> values)
    {
        foreach (var value in values)
            FillUser(value);
    }

    private static void FillUser(T value)
    {
        switch (value)
        {
            case IAddCompanyUser addCompanyUser:
                addCompanyUser.AddCompanyUser(new CompanyUser
                    { Company = new Company { Title = Guid.NewGuid().ToString() } });
                break;
            case Pilot.Capability.Models.ModelHelpers.IAddCompanyUser addUser:
                addUser.AddCompanyUser(value.Id);
                break;
        }
    }

    protected async Task<BaseModel> CreateUser(bool withAuthorization = true) 
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        if (ExistServiceContext(ServiceName.WorkerServer))
        {
            var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1, listDepth: 0).First();
            
            var context = GetContext(ServiceName.WorkerServer);
            await context.AddAsync(companyUser);
            await context.SaveChangesAsync();
            
            user.Id = companyUser.Id;
            
            var identityContext = GetContext(ServiceName.IdentityServer);
            await identityContext.AddRangeAsync(user);
            await identityContext.SaveChangesAsync();
            
            if (withAuthorization && TokenService != null)
            {
                var token = TokenService.GenerateToken(companyUser.Id, Role.Junior);
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            return companyUser;
        }
        
        var dbContext = GetContext(ServiceName.IdentityServer);
        await dbContext.AddRangeAsync(user);
        await dbContext.SaveChangesAsync();
    
        if (withAuthorization)
        {
            var token = TokenService.GenerateToken(user.Id, Role.Junior);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return user;
        
    }
    
    [Fact]
    public virtual async Task GetAllValuesTest_FilterWithIds_ReturnOk()
    {
        #region Arrange
    
        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        FillUser(values);
    
        var context = GetContext(_serviceName);
        await context.AddRangeAsync(values);
        await context.SaveChangesAsync();
    
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
    
    // [Fact]
    // public virtual async Task GetAllValuesTest_FilterWithWhereFilter_ReturnOk()
    // {
    //     #region Arrange
    //
    //     const int count = 3;
    //     var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
    //     FillUser(values);
    //
    //     var workerContext = GetContext(_serviceName);
    //     await workerContext.AddRangeAsync(values);
    //     await workerContext.SaveChangesAsync();
    //
    //     var filter = new BaseFilter
    //     {
    //         WhereFilter = new WhereFilter((nameof(BaseId.Id), values.Select(c => c.Id).First()))
    //     };
    //
    //     #endregion
    //
    //     // Act
    //     var result = await Client.GetAsync($"api/{EntityName}?filter={filter.ToJson()}");
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await result.Content.ReadFromJsonAsync<ICollection<TDto>>();
    //     Assert.NotNull(content);
    //     Assert.True(content.Count >= count);
    // }
    //
    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<T>(count: count, listDepth: 0);
        FillUser(values);

        var context = GetContext(_serviceName);
        await context.AddRangeAsync(values);
        await context.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}");
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

        var context = GetContext(_serviceName);
        await context.AddRangeAsync(values);
        await context.SaveChangesAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}/{id}");
        _testOutputHelper.WriteLine($"Status Code: {result.StatusCode}");
        _testOutputHelper.WriteLine($"Response: {await result.Content.ReadAsStringAsync()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<TDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    [Fact]
    public virtual async Task CreateModelTest_ReturnOk()
    {
        #region Arrange

        var valueModel = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        await GenerateTestEntity.FillChildren(valueModel, GetContext(_serviceName));

        var value = Mapper.Map<TDto>(valueModel);

        var userModel = await CreateUser(withAuthorization: false);
        
        #endregion

        // Act

        await Publisher.Publish(new CreateCommandMessage<TDto>(value, userModel.Id));
        await Helper.Wait();

        // Assert

        var result = await GetContext(_serviceName).Set<T>().Where(c => c.CreateAt == value.CreateAt)
            .FirstOrDefaultAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public virtual async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        var workerContext = GetContext(_serviceName);
        await workerContext.AddAsync(value);
        await workerContext.SaveChangesAsync();

        var valueDto = Mapper.Map<TDto>(value);
        var userModel = await CreateUser(withAuthorization: false);

        #endregion

        // Act

        await Publisher.Publish(new UpdateCommandMessage<TDto>(valueDto, userModel.Id));
        await Helper.Wait();

        // Assert

        var result = await GetContext(_serviceName).Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(result.ChangeAt);
    }

    [Fact]
    public virtual async Task DeleteModelTest_ReturnOk()
    {
        #region Arrange

        var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();

        var workerContext = GetContext(_serviceName);
        await workerContext.AddAsync(value);
        await workerContext.SaveChangesAsync();

        var userModel = await CreateUser(withAuthorization: false);

        #endregion

        // Act

        await Publisher.Publish(new DeleteCommandMessage<TDto>(value.Id, userModel.Id));
        await Helper.Wait();

        // Assert

        var result = await GetContext(_serviceName).Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.Null(result);
    }
}