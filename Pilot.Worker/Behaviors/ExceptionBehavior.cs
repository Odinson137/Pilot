using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Exception;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Worker.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;
    private readonly IMessageService _messageService;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger,
        IMessageService messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (MessageException e)
        {
            _logger.LogInformation($"Arise message exception {typeof(TRequest).Name}");

            var message = e.FromJson<MessageDto>();
            
            _logger.LogClassInfo(message);

            await _messageService.SendMessageAsync(message);

            throw;
        }
    }
}