using MassTransit;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.BackgroundJob.Consumers.ChatReminder;

public class ChatReminderDeleteConsumer(
    ILogger<ChatReminderDeleteConsumer> logger,
    IChatReminder repository,
    IMessageService messageService,
    IJob job,
    IValidatorService validate)
    : IConsumer<DeleteCommandMessage<ChatReminderDto>>
{
    // TODO в случае возникновения связанных сущностей будет возникать ошибка. Придумать как её потом обработать
    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<ChatReminderDto>> context)
    {
        logger.LogInformation($"{nameof(Models.ChatReminder)} delete consume");
        
        var id = context.Message.Value;
        var model = await validate.DeleteValidateAsync<Models.ChatReminder>(id, context.CancellationToken);
        
        repository.FastDelete(model);
        await repository.SaveAsync();
        
        foreach (var dayOfWeek in model.DayOfWeeks)
            job.RemoveReminderRecurringJob($"chatReminder-{id}-{dayOfWeek}");

        var message = new InfoMessageDto
        {
            MessagePriority = MessageInfo.Success | MessageInfo.Delete,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Models.ChatReminder>()
        };
        
        await messageService.SendInfoMessageAsync(message, context.Message.UserId);
    }
}