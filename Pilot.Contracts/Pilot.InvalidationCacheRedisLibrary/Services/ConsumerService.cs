using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.InvalidationCacheRedisLibrary.CacheKeyTemplates;
using Pilot.InvalidationCacheRedisLibrary.DTO;
using StackExchange.Redis;

namespace Pilot.InvalidationCacheRedisLibrary.Services;

public class ConsumerService(
    IConnectionMultiplexer connectionMultiplexer,
    ILogger<ConsumerService> logger,
    IRedisService redisService)
    : BackgroundService
{
    private readonly ISubscriber _subscriber = connectionMultiplexer.GetSubscriber();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Execute background service for invalidate cache");

        async void Handler(RedisChannel channel, RedisValue messageJson)
        {
            logger.LogInformation($"Consume invalidating cache from {channel} with message: {messageJson}");
            var message = messageJson.FromJson<InvalidateCacheMessage>();

            await redisService.DeleteValueAsync(BaseCacheKeyTemplate.OneCacheKey(message.Type, message.Id));

            await redisService.DeleteValuesAsync(message.Type);
        }

        await _subscriber.SubscribeAsync(RedisConnect.Channel, Handler);
    }
}