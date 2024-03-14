using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace Pilot.Tests.Identity.Tests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithUsername("root")
        .WithPassword("example")
        // .WithName("PilotIdentityTest")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(async services =>
        {
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