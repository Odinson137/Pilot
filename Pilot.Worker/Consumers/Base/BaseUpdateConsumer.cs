using MassTransit;
using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Notifications;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Worker.Consumers.Base;

public abstract class BaseUpdateConsumer<T, TDto>(
    ILogger<BaseUpdateConsumer<T, TDto>> logger,
    IMediator mediator)
    : IConsumer<UpdateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    public virtual async Task Consume(ConsumeContext<UpdateCommandMessage<TDto>> context)
    {
        logger.LogInformation($"{typeof(T).Name} update consume");

        var command = new UpdateEntityCommand<TDto>(context.Message.Value, context.Message.UserId);
        var model = await mediator.Send(command, context.CancellationToken);

        await mediator.Publish(new MessageSentNotification
        {
            Message = new InfoMessageDto
            {
                MessagePriority = MessageInfo.Success | MessageInfo.Update,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
                EntityId = model.Id
            },
            UserId = context.Message.UserId
        }, context.CancellationToken);
    }
}