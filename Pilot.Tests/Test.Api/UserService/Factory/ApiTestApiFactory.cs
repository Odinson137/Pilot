using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Api;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using Testcontainers.Redis;

namespace Test.Api.UserService.Factory;

public class ApiTestApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();
    
    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        Environment.SetEnvironmentVariable("RedisCache:ConnectionString",
            _redisContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            services.RemoveAll<IHttpIdentityService>(); 
            services.AddScoped<IHttpIdentityService, HttpIdentityServiceFaker>();
            services.RemoveAll<IModelService>();
            services.AddScoped<IModelService, ModelServiceFaker>();
        });
    }
}