using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Identity.Data;
using Testcontainers.MySql;
using Xunit;

namespace Pilot.Tests.Identity.Tests.IntegrationTests;

public class IntegrationIdentityTestWebAppFactory : WebApplicationFactory<Pilot.Identity.Program>, IAsyncLifetime
{
    // private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
    //     .WithImage("mongo:latest")
    //     .WithUsername("root")
    //     .WithPassword("example")
    //     .Build();
    //
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithImage("mysql:latest")
        .WithName("pilot_mysql")
        .WithUsername("root")
        .WithPassword("12345678")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(async services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            
            // services.AddTransient<ISeed, TestSeed>();

            var mongoDb = services.RemoveAll<MongoClient>();
            var mySqlDb = services.RemoveAll<DataContext>();
            services.AddMySql<DataContext>(
                "server=pilot_mysql;user=root;password=12345678;database=PilotIdentityDb;",
                new MySqlServerVersion(new Version()));
        });
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }
}