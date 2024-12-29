using Pilot.Contracts.Base;
using StackExchange.Redis;

namespace Pilot.Contracts.Interfaces;

public interface IRedisService
{
    public Task<T> GetValueAsync<T>(string key) where T : BaseDto;
    
    public Task<RedisValue> GetValueAsync(string key);

    public Task<ICollection<T>?> GetValuesAsync<T>(string key);
    
    public Task SetValueAsync<T>(string key, T value) where T : BaseDto;

    public Task SetValuesAsync<T>(string key, ICollection<T> values) where T : BaseDto;

    public Task DeleteValueAsync(string key);
    
    public Task DeleteValuesAsync(string key);

    Task AddQueueValueAsync<T>(string key, T value) where T : BaseDto;

    Task<long> GetQueueValuesCountAsync<T>(string key) where T : BaseDto;
    
    Task<ICollection<T>> GetQueueValuesAsync<T>(string key) where T : BaseDto;
}