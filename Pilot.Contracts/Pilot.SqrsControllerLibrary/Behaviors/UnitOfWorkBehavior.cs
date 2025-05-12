using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : BaseModel where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnitOfWorkBehavior<TRequest, TResponse>> _logger;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork, ILogger<UnitOfWorkBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        if (!(requestType.IsGenericType && 
              requestType.GetInterfaces().Any(i => 
                  i.IsGenericType && 
                  i.GetGenericTypeDefinition() == typeof(IEntityCommand<>))) || _unitOfWork.HasActiveTransaction)
        {
            return await next();
        }

        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return response;
            // await using var transaction = await _unitOfWork.StartTransactionAsync(cancellationToken);
            // TResponse response;
            // using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
            // {
            //     _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeof(TRequest).Name, request);
            //
            //     response = await next();
            //     await _unitOfWork.SaveChangesAsync(cancellationToken);
            //
            //     _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeof(TRequest).Name);
            //
            //     await _unitOfWork.EndTransactionAsync(cancellationToken);
        }

            // return response;
        // }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeof(TRequest).Name, request);

            throw;
        }
        
        // var response = await next();
        //

    }
}