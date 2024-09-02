using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Api.Handlers.BaseHandlers;

public abstract class ModelCommandHandler<TDto> : 
    IRequestHandler<CreateCommand<TDto>>,
    IRequestHandler<UpdateCommand<TDto>>,
    IRequestHandler<DeleteCommand<TDto>>
    where TDto : BaseDto
{
    private readonly IBaseMassTransitService _massTransitService;

    public ModelCommandHandler(IBaseMassTransitService massTransitService)
    {
        _massTransitService = massTransitService;
    }
    
    public async Task Handle(CreateCommand<TDto> request, CancellationToken cancellationToken)
    {
        await _massTransitService.Publish(
            new CreateCommandMessage<TDto>(request.ValueDto, request.UserId), cancellationToken);
    }

    public async Task Handle(UpdateCommand<TDto> request, CancellationToken cancellationToken)
    {
        await _massTransitService.Publish(
            new UpdateCommandMessage<TDto>(request.ValueDto, request.UserId), cancellationToken);
    }

    public async Task Handle(DeleteCommand<TDto> request, CancellationToken cancellationToken)
    {
        await _massTransitService.Publish(
            new DeleteCommandMessage<TDto>(request.ValueId, request.UserId), cancellationToken);
    }
}