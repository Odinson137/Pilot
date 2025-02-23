using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class NotificationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : BaseModel where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();


        return response;
    }
}