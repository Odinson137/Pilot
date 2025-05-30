﻿using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.BackgroundJob.Data;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Test.BackgroundJob.Factories;

public class BackgroundJobTestBackgroundJobFactory : WebApplicationFactory<Pilot.BackgroundJob.Program>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3")
        .Build();
    
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        Environment.SetEnvironmentVariable("RabbitMQ:ConnectionString",
            _rabbitContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("RedisCache:Endpoints",
            _redisContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            
            services.RemoveAll<DbContextOptions<DataContext>>();
            
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            services.RemoveAll<IBackgroundJobClient>();
            services.RemoveAll<IRecurringJobManager>();
            services.RemoveAll<IBackgroundProcessingServer>();

            services.AddHangfire(config =>
            {
                config.UseInMemoryStorage();
            });

            services.AddHangfireServer();
            
            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }

    public async Task InitializeAsync()
    {
        await _rabbitContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _rabbitContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}
