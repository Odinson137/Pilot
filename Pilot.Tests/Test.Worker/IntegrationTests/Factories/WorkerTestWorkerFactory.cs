﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Contracts.Data;
using Pilot.Worker.Data;
using Test.Base.IntegrationBase;
using Testcontainers.MySql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Test.Worker.IntegrationTests.Factories;

public class WorkerTestWorkerFactory : WebApplicationFactory<Pilot.Worker.Program>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

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
            
            services.RemoveAll<DbContextOptions<DataContext>>();
            
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });
        });
    }
}