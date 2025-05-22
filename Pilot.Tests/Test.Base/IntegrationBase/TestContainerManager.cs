using DotNet.Testcontainers.Containers;
using Testcontainers.ClickHouse;
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

    private static readonly ClickHouseContainer _clickHouseContainer = new ClickHouseBuilder()
        .WithImage("clickhouse/clickhouse-server:latest")
        .WithPortBinding(8123, 8123)
        .WithPortBinding(9000, 9000)
        .Build();
    
    public static string RedisConnectionString => _redisContainer.GetConnectionString();
    public static string RabbitMqConnectionString => _rabbitContainer.GetConnectionString();
    public static string ClickHouseConnectionString => _clickHouseContainer.GetConnectionString();

    public static async Task InitializeAsync(bool runRabbit = true, bool runRedis = true, bool runClickHouse = false)
    {
        if (runRabbit)
            await _rabbitContainer.StartAsync();
        if (runRedis)
            await _redisContainer.StartAsync();
        if (runClickHouse)
            await _clickHouseContainer.StartAsync();
    }

    public static async Task DisposeAsync()
    {
        if (_redisContainer.State == TestcontainersStates.Running)
            await _redisContainer.StopAsync();
        if (_rabbitContainer.State == TestcontainersStates.Running)
            await _rabbitContainer.StopAsync();
        if (_clickHouseContainer.State == TestcontainersStates.Running)
            await _clickHouseContainer.StartAsync();
    }
}