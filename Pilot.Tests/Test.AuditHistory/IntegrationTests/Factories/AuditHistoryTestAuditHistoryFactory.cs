using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.Contracts.Data;
using Pilot.Storage.Data;
using Test.Base.IntegrationBase;
using Testcontainers.RabbitMq;
using Testcontainers.ClickHouse;

namespace Test.AuditHistory.IntegrationTests.Factories;

public class AuditHistoryTestAuditHistoryFactory : WebApplicationFactory<Pilot.AuditHistory.Program>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management")
        .Build();

    private readonly ClickHouseContainer _clickHouseContainer = new ClickHouseBuilder()
        .WithImage("clickhouse/clickhouse-server:latest")
        .WithPortBinding(8123, 8123)
        .WithPortBinding(9000, 9000)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        Environment.SetEnvironmentVariable("RabbitMQ:ConnectionString",
            _rabbitContainer.GetConnectionString());

        Environment.SetEnvironmentVariable("ClickHouse:ConnectionString",
            _clickHouseContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>();
            services.AddTransient<ISeed, TestSeed>();

            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }

    public async Task InitializeAsync()
    {
        await _rabbitContainer.StartAsync();
        await _clickHouseContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _rabbitContainer.StopAsync();
        await _clickHouseContainer.StopAsync();
    }
}