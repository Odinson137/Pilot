using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Test.Base.IntegrationBase;

public static class TestContainerManager
{
    private static readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    private static readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management")
        .Build();

    public static string RedisConnectionString => _redisContainer.GetConnectionString();
    public static string RabbitMqConnectionString => _rabbitContainer.GetConnectionString();

    public static async Task InitializeAsync()
    {
        await _rabbitContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public static async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
        await _rabbitContainer.StopAsync();
    }
}