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
            // пока так. Позже придумать нормальную систему ключей
            var redisKey = BaseExpendMethods.GetModelName(response) + response.IdString;
            try
            {
                await redisService.DeleteValueAsync(redisKey);
            }
            catch (Exception e)
            {
                // пока так, чтоб я просто знал, что что-то здесь не работает.
                // Но в будущем надо сделать так, чтоб неуспешное удаление ключа из редиса не кидало ошибку
                throw new Exception("Redis in not working");
            }
        }

        return response;
    }
}