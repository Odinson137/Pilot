using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Pilot.Api.Services;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Identity.Models;
using Pilot.Tests.Api.Tests.IntegrationTests.Factories;
using Pilot.Tests.IntegrationBase;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests;

public class BaseModelIntegrationTest : BaseApiIntegrationTest
{
    public BaseModelIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) : base(apiFactory, receiverFactory, identityFactory)
    {
        var admin = new UserModel
        {
            UserName = "Admin",
            Name = "AdminName",
            LastName = "AdminLastName",
            Password = "12345678",
            Role = Role.Admin,
            Timestamp = DateTime.Now
        };

        IdentityContext.Add(admin);
        IdentityContext.SaveChanges();
        
        var tokenService = apiFactory.Services.GetRequiredService<IToken>();
        var token = tokenService.GenerateToken(admin.Id, admin.Role);
        
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static IEnumerable<object[]> ModelData
    {
        get
        {
            var baseModelType = typeof(BaseModel);
            var assembly = Assembly.GetAssembly(baseModelType);

            var modelTypes = assembly?.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(baseModelType))
                .Select(c => new object[] { c })
                .ToList();

            return modelTypes!;
        }
    }
    
    [Theory]
    [TestBeforeAfter]
    [MemberData(nameof(ModelData))]
    public async void GetAllValuesTest_ReturnOk(Type type, int count = 2)
    {
        #region Arrange

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
    
    [Theory]
    [TestBeforeAfter]
    [MemberData(nameof(ModelData))]
    public async void GetValue_ReturnOk(Type type, int count = 1)
    {
        #region Arrange

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