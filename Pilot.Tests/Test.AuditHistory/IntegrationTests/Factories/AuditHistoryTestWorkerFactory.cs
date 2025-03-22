using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.Contracts.Data;
using Pilot.Worker.Data;
using Test.Base.IntegrationBase;

namespace Test.AuditHistory.IntegrationTests.Factories;

public class AuditHistoryTestWorkerFactory : WebApplicationFactory<Pilot.Worker.Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>();
            services.AddTransient<ISeed, TestSeed>();

            services.RemoveAll<DbContextOptions<DataContext>>();
            services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestDatabase"); });

            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}