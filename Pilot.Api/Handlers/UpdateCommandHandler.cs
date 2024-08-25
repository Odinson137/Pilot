using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Handlers;

public class UpdateCommandHandler<TDto> :
    IRequestHandler<UpdateCommand<TDto>>
    where TDto : BaseDto
{
    private readonly ILogger<UpdateCommandHandler<TDto>> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public UpdateCommandHandler(ILogger<UpdateCommandHandler<TDto>> logger, IBaseMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(UpdateCommand<TDto> request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Update command handling {typeof(TDto).Name}");

        await _massTransitService.Publish(
            new UpdateCommandMessage<TDto>(request.ValueDto, request.UserId), cancellationToken);

        _logger.LogInformation($"Update command handled {typeof(TDto).Name}");
    }
}