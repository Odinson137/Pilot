using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Services;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Identity.Models;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;

namespace Test.Api.IntegrationTests;

public abstract class BaseModelIntegrationTest<T, TDto> : BaseApiIntegrationTest where T : BaseModel where TDto : BaseDto
{
    public BaseModelIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) : base(apiFactory, receiverFactory, identityFactory)
    {
        var admin = new User
        {
            UserName = $"Admin-{Guid.NewGuid()}",
            Name = "AdminName",
            LastName = "AdminLastName",
            Password = "12345678",
            Role = Role.Admin,
        };

        IdentityContext.Add(admin);
        IdentityContext.SaveChanges();
        
        var tokenService = apiFactory.Services.GetRequiredService<IToken>();
        var token = tokenService.GenerateToken(admin.Id, admin.Role);
        
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [Fact]
    public async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        var type = typeof(T);
        const int count = 2;
        
        var values = GenerateTestEntity.CreateEntities(type, count: count, listDepth: 0);
        
        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();
        
        #endregion

        // Act
        var result = await ApiClient.GetAsync($"api/{type.Name}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<BaseDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    [Fact]
    public async Task GetValue_ReturnOk()
    {
        #region Arrange

        var type = typeof(T);
        const int count = 2;
        
        var values = GenerateTestEntity.CreateEntities(type, count: count, listDepth: 0);
        
        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();

        var id = values.First().Id;
        
        #endregion

        // Act
        var result = await ApiClient.GetAsync($"api/{type.Name}/{id}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<BaseDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }
}