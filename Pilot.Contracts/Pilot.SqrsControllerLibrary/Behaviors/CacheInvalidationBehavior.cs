using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class CacheInvalidationBehavior<TRequest, TResponse>(IRedisService redisService)
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : BaseModel where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        var requestType = typeof(TRequest);
        if (requestType.IsGenericType && 
            requestType.GetInterfaces().Any(i => 
                i.IsGenericType && 
                i.GetGenericTypeDefinition() == typeof(IEntityCommand<>)))
        {
            await redisService.DeleteValueAsync(BaseExpendMethods.GetModelName<TResponse>() + response.IdString);
        }
        

        return response;
    }
}