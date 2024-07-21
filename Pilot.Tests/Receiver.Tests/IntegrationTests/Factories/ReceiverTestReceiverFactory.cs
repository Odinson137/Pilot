using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Contracts.Data;
using Pilot.Contracts.Services;
using Pilot.Tests.IntegrationBase;
using Testcontainers.MySql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests.Factories
{
    public class ReceiverTestReceiverFactory : WebApplicationFactory<Pilot.Receiver.Program>, IAsyncLifetime
    {
        private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithDatabase("TestPilotDb")
            .Build();
        
        private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .Build();
        
        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .Build();

        private const string ProjectTestName = "Receiver";
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ENVIRONMENT", ProjectTestName);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            
            Environment.SetEnvironmentVariable($"{ProjectTestName}.RabbitMQ:ConnectionString", _rabbitContainer.GetConnectionString());
            Environment.SetEnvironmentVariable($"{ProjectTestName}.MySql:ConnectionString", _mySqlContainer.GetConnectionString());
            Environment.SetEnvironmentVariable($"{ProjectTestName}.RedisCache:ConnectionString", _redisContainer.GetConnectionString());
            
            builder.ConfigureTestServices(async services =>
            {
                services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
                services.AddTransient<ISeed, TestSeed>();
            });
        }
        
        public async Task InitializeAsync()
        {
            await _rabbitContainer.StartAsync();
            await _redisContainer.StartAsync();
            await _mySqlContainer.StartAsync();

        }

        public new async Task DisposeAsync()
        {
            await _rabbitContainer.StopAsync();
            await _mySqlContainer.StopAsync();
            await _redisContainer.StopAsync();
        }
    }
}

