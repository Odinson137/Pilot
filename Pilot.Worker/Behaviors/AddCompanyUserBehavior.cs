using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.Worker.Interface;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Behaviors;

public class AddCompanyUserBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : BaseModel where TRequest : notnull
{
    private readonly ICompanyUser _companyUser;

    public AddCompanyUserBehavior(ICompanyUser companyUser)
    {
        _companyUser = companyUser;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (typeof(TRequest).IsGenericType && 
            typeof(TRequest).GetGenericTypeDefinition() == typeof(CreateEntityCommand<>))
        {
            if (response is not IAddCompanyUser addUser)
                return response;

            var entityCommand = (dynamic)request;
            var user = await _companyUser.GetByIdAsync(entityCommand.UserId, cancellationToken);
            if (user == null)
                throw new Exception($"User with id {entityCommand.UserId} does not exist");

            addUser.AddCompanyUser(user);
        }

        return response;
    }
}