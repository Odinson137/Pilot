using System.Net.Http.Json;
using MongoDB.Driver;
using Pilot.Receiver.Data;
using Pilot.Receiver.Models;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class CompanyIntegrationTest : BaseReceiverIntegrationTest
{
    private readonly IMongoCollection<Company> _collection;
    // private readonly IDistributedCache _redis;
    
    public CompanyIntegrationTest(IntegrationReceiverTestWebAppFactory factory) 
        : base(factory)
    {
        _collection = MongoDatabase.GetCollection<Company>(MongoTable.Company);
        // _redis = ScopeService.ServiceProvider.GetRequiredService<IDistributedCache>();
    }

    private async Task ClearMongoDb() => await _collection.DeleteManyAsync(FilterDefinition<Company>.Empty);
    // private async Task ClearRedis(string key) => await _redis.RemoveAsync(key);
    
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

        await ClearMongoDb();
        await _collection.InsertManyAsync(companies);

        // Act
        var request = await Client.GetAsync("Company");

        // Assert
        Assert.True(request.IsSuccessStatusCode);

        var companiesDb = await request.Content.ReadFromJsonAsync<ICollection<Company>>();
        
        Assert.NotNull(companiesDb);
        Assert.True(companiesDb.Count == 3);
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
        // await ClearRedis(CompanyController.GetCompanyCache(company.Id));
        await _collection.InsertOneAsync(company);

        // Act
        var request = await Client.GetAsync($"Company/{company.Id}");

        // Assert
        Assert.True(request.IsSuccessStatusCode);

        var companiesDb = await request.Content.ReadFromJsonAsync<Company>();
        
        Assert.NotNull(companiesDb);
        Assert.True(companiesDb.Title == company.Title);
    }
}