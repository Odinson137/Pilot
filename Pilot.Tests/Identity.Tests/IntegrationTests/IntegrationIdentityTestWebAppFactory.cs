using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Tests.IntegrationBase;
using Testcontainers.MongoDb;
using Xunit;

namespace Pilot.Tests.Identity.Tests.IntegrationTests;

public class IntegrationIdentityTestWebAppFactory : WebApplicationFactory<Pilot.Identity.Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithUsername("root")
        .WithPassword("example")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(async services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            
            services.AddTransient<ISeed, TestSeed>();

            var mongoDb = services.RemoveAll<MongoClient>();
            services.AddSingleton(new MongoClient(_mongoDbContainer.GetConnectionString()).GetDatabase("TestDb"));

        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}