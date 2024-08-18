using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace Pilot.Receiver.Service;

public class CacheInvalidationBackgroundService(IConnectionMultiplexer connectionMultiplexer, IMemoryCache memoryCache)
    : BackgroundService
{
    public const string Channel = "cache-invalidation";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = connectionMultiplexer.GetSubscriber();
        await subscriber.SubscribeAsync(Channel, (channel, key) =>
        {
            memoryCache.Remove(key);
        });
    }
}