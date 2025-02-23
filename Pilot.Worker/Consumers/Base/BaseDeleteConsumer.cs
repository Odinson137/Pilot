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

public abstract class BaseDeleteConsumer<T, TDto>(
    ILogger<BaseDeleteConsumer<T, TDto>> logger,
    IMediator mediator)
    : IConsumer<DeleteCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    // TODO в случае возникновения связанных сущностей будет возникать ошибка. Придумать как её потом обработать
    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<TDto>> context)
    {
        logger.LogInformation($"{typeof(T).Name} delete consume");
        
        var command = new DeleteEntityCommand<TDto>(context.Message.Value, context.Message.UserId);
        var model = await mediator.Send(command, context.CancellationToken);

        await mediator.Publish(new MessageSentNotification
        {
            Message = new InfoMessageDto
            {
                MessagePriority = MessageInfo.Success | MessageInfo.Delete,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
                EntityId = model.Id
            },
            UserId = context.Message.UserId
        }, context.CancellationToken);
    }
}