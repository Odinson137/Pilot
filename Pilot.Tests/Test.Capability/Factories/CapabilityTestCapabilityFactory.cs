using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.Capability.Data;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using Testcontainers.RabbitMq;

namespace Test.Capability.Factories;

public class CapabilityTestCapabilityFactory : WebApplicationFactory<Pilot.Capability.Program>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        Environment.SetEnvironmentVariable("RabbitMQ:ConnectionString",
            _rabbitContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            
            services.RemoveAll<DbContextOptions<DataContext>>();
            
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });
            
            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }

    public async Task InitializeAsync()
    {
        await _rabbitContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _rabbitContainer.StopAsync();
    }

}