using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pilot.Api.Controller;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO;
using Pilot.Receiver.Data;
using Pilot.Receiver.Models;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class CompanyUserIntegrationTests : BaseReceiverIntegrationTest
{
    private readonly IMongoCollection<Company> _collection;
    
    public CompanyUserIntegrationTests(IntegrationReceiverTestWebAppFactory factory) 
        : base(factory)
    {
        _collection = MongoDatabase.GetCollection<Company>(MongoTable.Company);
    } 
    
    private async Task ClearMongoDb() => await _collection.DeleteManyAsync(FilterDefinition<Company>.Empty);
    // private async Task ClearRedis(string key) => await _redis.RemoveAsync(key);
    
    [Fact]
    public async Task GetCompany_WithoutAuthorized_ShouldReturnNonAuthorized()
    {
        // Arrange
        UnAuthorize();
        
        // Act
        var request = await Client.GetAsync("CompanyUser/test");

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
                        Timestamp = DateTime.Today,
                        Role = CompanyUserRole.Owner,
                    },
                    new()
                    {
                        UserName = "Test",
                        Name = "Test",
                        LastName = "Test",
                        Timestamp = DateTime.Now,
                        Role = CompanyUserRole.Owner,
                    }
                }
            },
        };
        
        await ClearMongoDb();
        await _collection.InsertManyAsync(companies);

        Authorize();
        
        // Act
        var request = await Client.GetAsync($"CompanyUser/{companies.First().Id}");

        // Assert
        var _ = await request.Content.ReadAsStringAsync();
        Assert.True(request.IsSuccessStatusCode);

        var companyUsersDto = await request.Content.ReadFromJsonAsync<ICollection<CompanyUserDto>>();
        
        Assert.NotNull(companyUsersDto);
        Assert.True(companyUsersDto.Count == 2);
    }
    
   
}