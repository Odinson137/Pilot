using MediatR;
using Pilot.Api.Data;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;

namespace Pilot.Api.Behaviors;

public class CreateCommandHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly ILogger<CreateCommandHandling<TRequest, TResponse>> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public CreateCommandHandling(ILogger<CreateCommandHandling<TRequest, TResponse>> logger, IBaseMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Create command handling {typeof(TRequest).Name}");

        await _massTransitService.Publish(
            new CreateCommandMessage<TResponse>((TResponse)request.ValueDto, request.UserId), cancellationToken);
        // var response = await next();
        
        _logger.LogInformation($"Create command handled {typeof(TRequest).Name}");
        return (TResponse)request.ValueDto;
    }
}