using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Services;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Identity.Models;
using Pilot.Tests.Api.Tests.IntegrationTests.Factories;
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
    
    [Fact]
    public async void TestTest3()
    {
        var result = await ReceiverClient.GetAsync("api/Company");
        Assert.True(result.IsSuccessStatusCode);
    }
    
    [Fact]
    [TestBeforeAfter]
    public async void TestTest()
    {
        #region Arrange
        
        var company = new Company
        {
            Title = "Test",
            Description = "Test Description",
            CompanyUsers = new List<CompanyUser>
            {
                new()
                {
                    UserName = "Test user name",
                    Name = "Test name",
                    LastName = "Test LastName",
                    Timestamp = DateTime.Now
                }
            }
        };
        
        await ReceiverContext.AddAsync(company);
        await ReceiverContext.SaveChangesAsync();
        
        #endregion

        // Act
        var result = await ApiClient.GetAsync("api/Company");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<CompanyDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count == 1);
    }
}