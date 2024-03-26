using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Tests.IntegrationBase;
using Serilog;
using Testcontainers.MongoDb;
using Testcontainers.Redis;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests;

public class IntegrationApiTestWebAppFactory : WebApplicationFactory<Pilot.Api.Program>, IAsyncLifetime
{

    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithUsername("root")
        .WithPassword("example")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logger =>
        {
            logger.ClearProviders();
            logger.AddSerilog(new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.Console()
                .CreateLogger());
        });
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests

            services.RemoveAll<MongoClient>();
            services.RemoveAll<IDistributedCache>();

            services.AddTransient<ISeed, TestSeed>();
            
            services.AddSingleton(new MongoClient(_mongoDbContainer.GetConnectionString()).GetDatabase("TestDb"));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
                options.InstanceName = "Test:";
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
        await _mongoDbContainer.StartAsync();

    }

    public new async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
        await _mongoDbContainer.StopAsync();
    }
}
