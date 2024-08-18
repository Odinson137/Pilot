using Pilot.Contracts.Data.Enums;

namespace Pilot.InvalidationCacheRedisLibrary.Interfaces;

public interface IInvalidateCacheService
{
    public Task PublishAsync<T>(int id, ActionState operation);

    public Task PublishAsync(Type type, int id, ActionState operation);
}