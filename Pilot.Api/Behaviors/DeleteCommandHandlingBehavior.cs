using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Behaviors;

public class DeleteCommandHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<DeleteCommandHandling<TRequest, TResponse>> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public DeleteCommandHandling(ILogger<DeleteCommandHandling<TRequest, TResponse>> logger,
        IBaseMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Delete command handling {typeof(TRequest).Name}");

        await _massTransitService.Publish(
            new DeleteCommandMessage<TResponse>((TResponse)request.ValueDto, request.UserId), cancellationToken);
        // var response = await next();

        _logger.LogInformation($"Delete command handled {typeof(TRequest).Name}");
        return (TResponse)request.ValueDto;
    }
}