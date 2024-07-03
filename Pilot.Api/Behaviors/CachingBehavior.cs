using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pilot.Contracts.Base;

namespace Pilot.Api.Behaviors;

public interface ICacheableMediatrQuery 
{
    public BaseFilter? Filter { get; set; }
    string CacheKey { get; }
    // TimeSpan? SlidingExpiration { get; }
}

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery
{
    private readonly IDistributedCache  _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    
    public CachingBehavior(IDistributedCache  cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var cashKey = request.CacheKey;
        var data = await _cache.GetStringAsync(cashKey, cancellationToken);
        if (data != null)
        {
            _logger.LogInformation("From cache");
            var memoryData = JsonConvert.DeserializeObject<TResponse>(data)!;
            return memoryData;
        }

        _logger.LogInformation("No cache");
        var response = await next.Invoke();
        await _cache.SetStringAsync(
            cashKey, 
            JsonConvert.SerializeObject(response),
            cancellationToken).ConfigureAwait(false);
        return response;
    }
}