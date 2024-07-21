using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Contracts.Data;
using Pilot.Tests.IntegrationBase;
using Testcontainers.Redis;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests.Factories;

public class ApiTestApiFactory : WebApplicationFactory<Pilot.Api.Program>, IAsyncLifetime
{
    //
    // private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
    //     .WithImage("mongo:latest")
    //     .WithUsername("root")
    //     .WithPassword("example")
    //     .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();
    
    private const string ProjectTestName = "Api";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ENVIRONMENT", ProjectTestName);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        
        Environment.SetEnvironmentVariable($"{ProjectTestName}.RedisCache:ConnectionString", _redisContainer.GetConnectionString());

        builder.ConfigureTestServices(async services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
        });
    }

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
        // await _mongoDbContainer.StartAsync();

    }

    public new async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
        // await _mongoDbContainer.StopAsync();
    }
}
