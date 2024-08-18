using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.InvalidationCacheRedisLibrary.Services;
using StackExchange.Redis;

namespace Pilot.InvalidationCacheRedisLibrary;

public static class RedisConnect
{
    public const string Channel = "invalidation-cache";

    public static async Task AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redis = configuration.GetConnection("RedisCache:ConnectionString")!;

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redis;
            options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
        });

        var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(redis);
        
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddSingleton(connectionMultiplexer.GetSubscriber);
        services.AddSingleton(connectionMultiplexer.GetDatabase());
        services.AddSingleton<IRedisService, RedisService>();
        services.AddHostedService<ConsumerService>();
    }
}