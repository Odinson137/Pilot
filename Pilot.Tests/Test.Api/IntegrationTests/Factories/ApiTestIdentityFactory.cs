﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using Testcontainers.MySql;

namespace Test.Api.IntegrationTests.Factories;

public class ApiTestIdentityFactory : WebApplicationFactory<Pilot.Identity.Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("TestPilotIdentityDb")
        .Build();
    
    private const string ProjectTestName = "Api";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ENVIRONMENT", ProjectTestName);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        
        Environment.SetEnvironmentVariable($"{ProjectTestName}.MySqlIdentity:ConnectionString", _mySqlContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
        });
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
        // await _mongoDbContainer.StartAsync();

    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
        // await _mongoDbContainer.StopAsync();
    }
}
