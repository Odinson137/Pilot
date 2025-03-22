using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using Testcontainers.ClickHouse;
using Program = Pilot.AuditHistory.Program;

namespace Test.Api.AuditHistoryService.Factory;

public class AuditHistoryTestAuditHistoryFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly ClickHouseContainer _clickHouseContainer = new ClickHouseBuilder()
        .WithImage("clickhouse/clickhouse-server:latest")
        .WithPortBinding(8123, 8123)
        .WithPortBinding(9000, 9000)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        
        Environment.SetEnvironmentVariable("ClickHouse:ConnectionString",
            _clickHouseContainer.GetConnectionString());
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();

            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }

    public async Task InitializeAsync()
    {
        await _clickHouseContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _clickHouseContainer.StopAsync();
    }
}