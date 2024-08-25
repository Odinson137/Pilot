using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Handlers;

public class DeleteCommandHandler<TDto> :
    IRequestHandler<DeleteCommand<TDto>>
    where TDto : BaseDto
{
    private readonly ILogger<DeleteCommandHandler<TDto>> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public DeleteCommandHandler(ILogger<DeleteCommandHandler<TDto>> logger, IBaseMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(DeleteCommand<TDto> request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Update command handling {typeof(TDto).Name}");

        await _massTransitService.Publish(
            new DeleteCommandMessage<TDto>(request.ValueId, request.UserId), cancellationToken);

        _logger.LogInformation($"Update command handled {typeof(TDto).Name}");
    }
}