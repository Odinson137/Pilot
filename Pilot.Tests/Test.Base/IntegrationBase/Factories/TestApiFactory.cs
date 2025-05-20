using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Pilot.Api;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase.Fakers;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Test.Base.IntegrationBase.Factories;

public class TestApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // private readonly RedisContainer _redisContainer = new RedisBuilder()
    //     .WithImage("redis:latest")
    //     .Build();
    //
    // private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
    //     .WithImage("rabbitmq:3-management")
    //     .Build();
    
    public async Task InitializeAsync()
    {
        await TestContainerManager.InitializeAsync();
        // await _rabbitContainer.StartAsync();
        // await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await TestContainerManager.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        // Environment.SetEnvironmentVariable("RabbitMQ:ConnectionString",
        //     _rabbitContainer.GetConnectionString());
        //
        // Environment.SetEnvironmentVariable("RedisCache:Endpoints",
        //     _redisContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            
            services.RemoveAll<IBaseHttpService>(); 
            services.AddScoped<IBaseHttpService, BaseHttpServiceFaker>();
            services.RemoveAll<IModelService>(); 
            services.AddScoped<IModelService, ModelServiceFaker>();
            
            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "RabbitMQ:ConnectionString", TestContainerManager.RabbitMqConnectionString },
                { "RedisCache:Endpoints", TestContainerManager.RedisConnectionString },
            }!);
        });
        return base.CreateHost(builder);
    }
}