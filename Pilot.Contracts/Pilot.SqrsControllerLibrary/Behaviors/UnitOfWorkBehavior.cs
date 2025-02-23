using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : BaseModel where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

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
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return response;
    }
}