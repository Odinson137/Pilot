using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class CachingListBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ICollection<TResponse>>
    where TRequest : ICacheableListMediatrQuery where TResponse : BaseDto
{
    private readonly IRedisService _redis;
    private readonly ILogger<CachingListBehavior<TRequest, TResponse>> _logger;

    public CachingListBehavior(ILogger<CachingListBehavior<TRequest, TResponse>> logger, IRedisService redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public async Task<ICollection<TResponse>> Handle(
        TRequest request, 
        RequestHandlerDelegate<ICollection<TResponse>> next, 
        CancellationToken cancellationToken)
    {
        var cashKey = request.CacheKey;
        var data = await _redis.GetValuesAsync<TResponse>(cashKey);
        if (data != null)
        {
            _logger.LogInformation("From cache");
            return data;
        }

        _logger.LogInformation("No cache");
        var response = await next.Invoke();
        await _redis.SetValuesAsync(cashKey, response);
        
        return response;
    }
}