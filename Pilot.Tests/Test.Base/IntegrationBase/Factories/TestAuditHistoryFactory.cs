﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Pilot.Contracts.Data;

namespace Test.Base.IntegrationBase.Factories;

public class TestAuditHistoryFactory : WebApplicationFactory<Pilot.AuditHistory.Program>, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await TestContainerManager.InitializeAsync(runClickHouse: true);
    }

    public new async Task DisposeAsync()
    {
        await TestContainerManager.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            
            // services.RemoveAll<DbContextOptions<ClickHouseContext>>();
            //
            // services.AddDbContext<ClickHouseContext>(options =>
            // {
            //     options.UseInMemoryDatabase("TestDatabase");
            // });

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
                { "ClickHouse:ConnectionString", TestContainerManager.ClickHouseConnectionString },
            }!);
        });
        return base.CreateHost(builder);
    }
}