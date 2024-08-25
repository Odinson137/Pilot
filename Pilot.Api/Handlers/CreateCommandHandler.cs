using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Handlers;

public class CreateCommandHandler<TDto> :
    IRequestHandler<CreateCommand<TDto>>
    where TDto : BaseDto
{
    private readonly ILogger<CreateCommandHandler<TDto>> _logger;
    private readonly IBaseMassTransitService _massTransitService;

    public CreateCommandHandler(ILogger<CreateCommandHandler<TDto>> logger, IBaseMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(CreateCommand<TDto> request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Create command handling {typeof(TDto).Name}");

        await _massTransitService.Publish(
            new CreateCommandMessage<TDto>(request.ValueDto, request.UserId), cancellationToken);

        _logger.LogInformation($"Create command handled {typeof(TDto).Name}");
    }
}