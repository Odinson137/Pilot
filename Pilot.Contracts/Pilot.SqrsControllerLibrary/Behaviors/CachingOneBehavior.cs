using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class CachingOneBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableOneMediatrQuery where TResponse : BaseDto
{
    private readonly IRedisService _redis;
    private readonly ILogger<CachingOneBehavior<TRequest, TResponse>> _logger;

    public CachingOneBehavior(ILogger<CachingOneBehavior<TRequest, TResponse>> logger, IRedisService redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cashKey = request.CacheKey;
        var data = await _redis.GetValueAsync(cashKey);
        if (data.HasValue)
        {
            _logger.LogInformation("From cache");
            var memoryData = data.FromJson<TResponse>();
            return memoryData;
        }

        _logger.LogInformation("No cache");
        var response = await next.Invoke();
        await _redis.SetValueAsync(cashKey, response);
        
        return response;
    }
}