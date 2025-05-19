using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Services;
using StackExchange.Redis;

namespace Pilot.SqrsControllerLibrary;

public static class RedisConnect
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("RedisCache");
        if (!redisConfig.GetChildren().Any())
            throw new InvalidOperationException("RedisCache is not configured.");

        var endpoints = redisConfig.GetValue<string>("Endpoints")?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        ?? throw new InvalidOperationException("RedisCache:Endpoints is not configured.");

        var configurationOptions = new ConfigurationOptions
        {
            DefaultDatabase = redisConfig.GetValue("DefaultDatabase", 0),
            AllowAdmin = redisConfig.GetValue("AllowAdmin", true),
            ConnectTimeout = redisConfig.GetValue("ConnectTimeout", 5000),
            SyncTimeout = redisConfig.GetValue("SyncTimeout", 5000),
            CommandMap = CommandMap.Default
        };

        foreach (var endpoint in endpoints.Select(e => e.Trim()))
            configurationOptions.EndPoints.Add(endpoint);

        // не нужно
        // var redis = configuration["RedisCache:ConnectionString"]!;
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = redis;
        //     options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
        // });

        // синхронный вариант подключения
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
        
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddSingleton(connectionMultiplexer.GetSubscriber());
        services.AddSingleton(connectionMultiplexer.GetDatabase());
        services.AddSingleton<IRedisService, RedisService>();
    }
}