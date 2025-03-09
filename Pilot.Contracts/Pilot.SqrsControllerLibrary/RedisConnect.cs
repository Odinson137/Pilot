using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Services;
using StackExchange.Redis;

namespace Pilot.SqrsControllerLibrary;

public static class RedisConnect
{
    public const string Channel = "invalidation-cache";

    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redis = configuration["RedisCache:ConnectionString"]!;

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redis;
            options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
        });

        // синхронный вариант подключения
        var connectionMultiplexer = ConnectionMultiplexer.Connect(redis);
        
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddSingleton(connectionMultiplexer.GetSubscriber);
        services.AddSingleton(connectionMultiplexer.GetDatabase());
        services.AddSingleton<IRedisService, RedisService>();
    }
}