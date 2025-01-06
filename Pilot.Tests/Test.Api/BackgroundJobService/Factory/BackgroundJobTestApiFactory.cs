using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Pilot.Api;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Serilog;
using Test.Base.IntegrationBase;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;
using Xunit.Abstractions;

namespace Test.Api.BackgroundJobService.Factory;

public class BackgroundJobTestApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();
    
    private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3")
        .Build();
    
    public async Task InitializeAsync()
    {
        await _rabbitContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
        await _rabbitContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        Environment.SetEnvironmentVariable("RabbitMQ:ConnectionString",
            _rabbitContainer.GetConnectionString());
        
        Environment.SetEnvironmentVariable("RedisCache:ConnectionString",
            _redisContainer.GetConnectionString());
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            
            services.RemoveAll<IBaseHttpService>(); 
            services.AddScoped<IBaseHttpService, BaseHttpServiceFaker>();
            services.RemoveAll<IModelService>(); 
            services.AddScoped<IModelService, ModelServiceFaker>();
        });
    }
}