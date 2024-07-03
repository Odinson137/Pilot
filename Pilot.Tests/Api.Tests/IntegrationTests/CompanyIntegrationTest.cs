using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests;

public class CompanyIntegrationTest : BaseApiIntegrationTest
{
    private readonly IMongoCollection<Company> _collection;
    private readonly IDistributedCache _redis;
    
    public CompanyIntegrationTest(IntegrationApiTestWebAppFactory factory) 
        : base(factory)
    {
        _collection = MongoDatabase.GetCollection<Company>(MongoTable.Company);
        _redis = ScopeService.ServiceProvider.GetRequiredService<IDistributedCache>();
    }

    private async Task ClearMongoDb()
    {
        await _collection.DeleteManyAsync(FilterDefinition<Company>.Empty);
    }
    
    private async Task ClearRedis(string key)
    {
        await _redis.RemoveAsync(key);

    }
    
    [Fact]
    public async Task GetCompany_ReturnAllCompanies_ShouldReturnOK()
    {
        // Arrange
        var companies = new List<Company>()
        {
            new ()
            {
                Title = "Apple",
                Description = "Small company for creating new types computers",
            },
            new ()
            {
                Title = "Microsoft",
                Description = "My favorite corporation",
            },
            new ()
            {
                Title = "Amazon",
                Description = "Target company",
            }
        };

        await _collection.InsertManyAsync(companies);

        // Act
        var request = await Client.GetAsync("api/Company");

        // Assert
        Assert.True(request.IsSuccessStatusCode);

        var companiesDb = await request.Content.ReadFromJsonAsync<ICollection<CompanyDto>>();
        
        Assert.NotNull(companiesDb);
        Assert.True(companiesDb.Count == 3);
    }
    
    [Fact]
    public async Task GetCompany_ReturnAllCompanies_FromCache_ShouldReturnOK()
    {
        // Arrange
        var companies = new List<Company>()
        {
            new ()
            {
                Title = "Apple",
                Description = "Small company for creating new types computers",
            },
            new ()
            {
                Title = "Microsoft",
                Description = "My favorite corporation",
            },
            new ()
            {
                Title = "Amazon",
                Description = "Target company",
            }
        };

        await ClearMongoDb();
        await ClearRedis("all-companies");
        await _collection.InsertManyAsync(companies);

        // Act 
        var requestDb = await Client.GetAsync("api/Company");
        
        var requestCache = await Client.GetAsync("api/Company");

        // Assert
        Assert.True(requestDb.IsSuccessStatusCode);
        Assert.True(requestCache.IsSuccessStatusCode);
        
        var redisCache = await _redis.GetStringAsync("all-companies");
        Assert.NotNull(redisCache);
        
        var companiesDb = await requestDb.Content.ReadFromJsonAsync<ICollection<CompanyDto>>();

        var companiesStringDb = await requestDb.Content.ReadAsStringAsync();
        var companiesStringCache = await requestCache.Content.ReadAsStringAsync();
        
        Assert.NotNull(companiesDb);
        Assert.True(companiesDb.Count == 3);
        Assert.Equal(companiesStringDb, companiesStringCache);
    }
    
    [Fact]
    public async Task GetCompany_ReturnOneCompanyById_ShouldReturnOK()
    {
        // Arrange
        var company = new Company()
        {
            Title = "Apple",
            Description = "Small company for creating new types computers",
        };
        
        await ClearMongoDb();
        await ClearRedis($"companyId-{company.Id}");
        await _collection.InsertOneAsync(company);

        // Act
        var request = await Client.GetAsync($"api/Company/{company.Id}");

        // Assert
        Assert.True(request.IsSuccessStatusCode);

        var companiesDb = await request.Content.ReadFromJsonAsync<CompanyDto>();
        
        Assert.NotNull(companiesDb);
        Assert.True(companiesDb.Title == company.Title);
    }
    
    [Fact]
    public async Task GetCompany_ReturnOneCompanyByIdFromCache_ShouldReturnOK()
    {
        // Arrange
        var company = new Company()
        {
            Title = "Apple",
            Description = "Small company for creating new types computers",
        };
        
        await ClearMongoDb();
        await ClearRedis($"companyId-{company.Id}");
        await _collection.InsertOneAsync(company);

        // Act
        var requestFromDb = await Client.GetAsync($"api/Company/{company.Id}");
        
        var requestFromCache = await Client.GetAsync($"api/Company/{company.Id}");

        // Assert
        Assert.True(requestFromDb.IsSuccessStatusCode);
        Assert.True(requestFromCache.IsSuccessStatusCode);

        var companiesDb = await requestFromDb.Content.ReadFromJsonAsync<CompanyDto>();
        var companiesCache = await requestFromCache.Content.ReadFromJsonAsync<CompanyDto>();
        
        Assert.NotNull(companiesDb);
        Assert.NotNull(companiesCache);
    }
}