using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using StackExchange.Redis;

namespace Pilot.SqrsControllerLibrary.Services;

public class RedisService(IDatabase redis) : IRedisService
{
    public async Task<T> GetValueAsync<T>(string key) where T : BaseDto
    {
        var valueJson = await GetValueAsync(key);
        return valueJson.FromJson<T>();
    }

    public async Task<string> GetValueAsync(string key)
    {
        var value = await redis.StringGetAsync(key);
        return value.HasValue ? value.ToString() : string.Empty;
    }

    public async Task<ICollection<T>?> GetValuesAsync<T>(string key)
    {
        var values = (await redis.HashValuesAsync(key))
            .Select(c => new RedisKey(c.ToString())).ToArray();
        
        if (!values.Any()) return null;
        
        var redisValues = await redis.StringGetAsync(values);

        if (redisValues.Any(c => c.IsNull)) return null;
        
        var list = redisValues.Select(c => c.FromJson<T>()).ToList();
        
        return list;
    }
    
    public async Task SetValueAsync<T>(string key, T value) where T : BaseDto
    {
        await redis.StringSetAsync(key, value.ToJson(), flags: CommandFlags.FireAndForget);
    }
    
    public async Task SetValuesAsync<T>(string key, ICollection<T> values) where T : BaseDto
    {
        var hashEntries = values.Select(c => new HashEntry(c.Id, string.Empty)).ToArray();
    
        await redis.HashSetAsync(key, hashEntries, flags: CommandFlags.FireAndForget);

        var keyValuePairs = values
            .ToDictionary(c => (RedisKey)c.IdString, c => (RedisValue)c.ToJson())
            .ToArray();

        await redis.SetAddAsync(typeof(T).Name, key, flags: CommandFlags.FireAndForget);
        
        await redis.StringSetAsync(keyValuePairs, flags: CommandFlags.FireAndForget);
    }

    
    public async Task DeleteValueAsync(string key)
    {
        await redis.KeyDeleteAsync(key, flags: CommandFlags.FireAndForget);
    }

    public async Task DeleteValuesAsync(string type)
    {
        var members = await redis.SetMembersAsync(type);

        var redisValues = members.Select(c => new RedisKey(c)).ToArray();
        
        await redis.KeyDeleteAsync(redisValues, flags: CommandFlags.FireAndForget);
    }
    
    public async Task AddQueueValueAsync<T>(string key, T value) where T : BaseDto
    {
        await redis.ListRightPushAsync($"queue-{typeof(T).Name}-{key}", value.ToJson());
    }
    
    public Task<long> GetQueueValuesCountAsync<T>(string key) where T : BaseDto
    {
        return redis.ListLengthAsync($"queue-{typeof(T).Name}-{key}");
    }

    public async Task<ICollection<T>> GetQueueValuesAsync<T>(string key) where T : BaseDto
    {
        var values = new List<T>();
        var fullKey = $"queue-{typeof(T).Name}-{key}";
        for (;;)
        {
            RedisValue? redisValue = await redis.ListLeftPopAsync(fullKey);
            if (redisValue.Value.HasValue)
                values.Add(redisValue.Value!.FromJson<T>());
            else break;
        }

        return values;
    }
}