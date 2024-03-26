using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pilot.Api.Controller;
using Pilot.Api.DTO;
using Pilot.Contracts.Data;
using Pilot.Contracts.Models;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests;

public class CompanyUserIntegrationTests : BaseApiIntegrationTest
{
    private readonly IMongoCollection<Company> _collection;
    private readonly IDistributedCache _redis;
    
    public CompanyUserIntegrationTests(IntegrationApiTestWebAppFactory factory) 
        : base(factory)
    {
        _collection = MongoDatabase.GetCollection<Company>(MongoTable.Company);
        _redis = ScopeService.ServiceProvider.GetRequiredService<IDistributedCache>();
    } 
    
    private async Task ClearMongoDb() => await _collection.DeleteManyAsync(FilterDefinition<Company>.Empty);
    private async Task ClearRedis(string key) => await _redis.RemoveAsync(key);
    
    [Fact]
    public async Task GetCompany_WithoutAuthorized_ShouldReturnNonAuthorized()
    {
        // Arrange
        UnAuthorize();
        
        // Act
        var request = await Client.GetAsync($"api/CompanyUser/test");

        // Assert
        Assert.True(!request.IsSuccessStatusCode);
        Assert.Equal(request.StatusCode,HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetCompany_ReturnAllCompanies_ShouldReturnOK()
    {
        // Arrange
        var companies = new List<Company>()
        {
            new ()
            {
                Title = "TestTitle",
                CompanyUsers = new List<CompanyUser>()
                {
                    new()
                    {
                        UserName = "Test",
                        Name = "Test",
                        LastName = "Test",
                        Timestamp = DateTime.Today
                    },
                    new()
                    {
                        UserName = "Test",
                        Name = "Test",
                        LastName = "Test",
                        Timestamp = DateTime.Now,
                    }
                }
            },
        };
        
        await ClearMongoDb();
        await ClearRedis(CompanyUserController.GetAllCompanyUsersCache(companies.First().Id));
        await _collection.InsertManyAsync(companies);

        Authorize();
        
        // Act
        var request = await Client.GetAsync($"api/CompanyUser/{companies.First().Id}");

        // Assert
        var _ = await request.Content.ReadAsStringAsync();
        Assert.True(request.IsSuccessStatusCode);

        var companyUsersDto = await request.Content.ReadFromJsonAsync<ICollection<CompanyUserDto>>();
        
        Assert.NotNull(companyUsersDto);
        Assert.True(companyUsersDto.Count == 2);
    }
    
    [Fact]
    public async Task GetCompany_ReturnAllCompaniesFromCache_ShouldReturnOK()
    {
        // Arrange
        var companies = new List<Company>()
        {
            new ()
            {
                Title = "TestTitle",
                CompanyUsers = new List<CompanyUser>()
                {
                    new()
                    {
                        UserName = "Test",
                        Name = "Test",
                        LastName = "Test",
                        Timestamp = DateTime.Today
                    },
                    new()
                    {
                        UserName = "Test",
                        Name = "Test",
                        LastName = "Test",
                        Timestamp = DateTime.Now,
                    }
                }
            },
        };
        
        await ClearMongoDb();
        await ClearRedis(CompanyUserController.GetAllCompanyUsersCache(companies.First().Id));
        await _collection.InsertManyAsync(companies);

        Authorize();
        
        // Act
        var request = await Client.GetAsync($"api/CompanyUser/{companies.First().Id}");
        
        var requestFromCache = await Client.GetAsync($"api/CompanyUser/{companies.First().Id}");

        // Assert
        Assert.True(request.IsSuccessStatusCode);
        Assert.True(requestFromCache.IsSuccessStatusCode);

        var companyUsersDto = await request.Content.ReadAsStringAsync();
        var companyUsersDtoFromCache = await requestFromCache.Content.ReadAsStringAsync();
        
        Assert.True(companyUsersDto == companyUsersDtoFromCache);
    }
}