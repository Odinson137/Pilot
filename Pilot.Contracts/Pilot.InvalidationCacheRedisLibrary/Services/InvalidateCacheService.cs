using Microsoft.Extensions.Logging;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;
using Pilot.InvalidationCacheRedisLibrary.DTO;
using Pilot.InvalidationCacheRedisLibrary.Interfaces;
using StackExchange.Redis;

namespace Pilot.InvalidationCacheRedisLibrary.Services;

public class InvalidateCacheService(
    ISubscriber subscriber, 
    ILogger<InvalidateCacheService> logger
    ) : IInvalidateCacheService
{
    public Task PublishAsync<T>(int id, ActionState operation)
    {
        return PublishAsync(typeof(T), id, operation);
    }
    
    public async Task PublishAsync(Type type, int id, ActionState operation)
    {
        logger.LogInformation("Publish invalidate notice");

        var message = new InvalidateCacheMessage
        {
            Type = type.Name,
            Id = id,
            Operation = operation
        };

        await subscriber.PublishAsync(RedisConnect.Channel, message.ToJson());
    }
}