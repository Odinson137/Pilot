using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class AddUserBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : BaseModel where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();
        if (typeof(TRequest).IsGenericType &&
            typeof(TRequest).GetGenericTypeDefinition() == typeof(CreateEntityCommand<>))
        {
            if (response is not IAddUser addUser)
                return response;

            var entityCommand = (dynamic)request;
            addUser.AddUser(entityCommand.UserId);
        }

        return response;
    }
}