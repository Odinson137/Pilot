using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Contracts.Data;
using Pilot.Tests.IntegrationBase;
using Testcontainers.MySql;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests.Factories
{
    public class ApiTestReceiverFactory : WebApplicationFactory<Pilot.Receiver.Program>, IAsyncLifetime
    {
        private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithPortBinding(3308, 3306)
            .WithDatabase("TestPilotDb")
            .Build();
    
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("MySql:ConnectionString", _mySqlContainer.GetConnectionString());
    
            builder.ConfigureTestServices(async services =>
            {
                services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
                services.AddTransient<ISeed, TestSeed>();
                services.RemoveAll<IDistributedCache>();
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
}

