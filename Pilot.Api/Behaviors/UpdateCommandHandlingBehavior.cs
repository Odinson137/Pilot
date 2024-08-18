using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Behaviors;

public class UpdateCommandHandling<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<UpdateCommandHandling<TRequest, TResponse>> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public UpdateCommandHandling(ILogger<UpdateCommandHandling<TRequest, TResponse>> logger,
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
        _logger.LogInformation($"Update command handling {typeof(TRequest).Name}");

        await _massTransitService.Publish(
            new UpdateCommandMessage<TResponse>((TResponse)request.ValueDto, request.UserId), cancellationToken);
        // var response = await next();

        _logger.LogInformation($"Update command handled {typeof(TRequest).Name}");
        return (TResponse)request.ValueDto;
    }
}