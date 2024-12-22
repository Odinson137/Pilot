using MassTransit;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
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
        logger.LogClassInfo(context.Message);

        var id = context.Message.Value;
        var model = await validate.DeleteValidateAsync<Models.ChatReminder>(id);
        
        await repository.FastDeleteAsync(model);

        foreach (var dayOfWeek in model.DayOfWeeks)
        {
            var jobId = $"chatReminder-{id}-{dayOfWeek}";
            job.RemoveReminderRecurringJob(jobId);
        }
        var message = new InfoMessageDto
        {
            Title = "Успешное удаление!",
            Description = $"Успешное удаление сущности {nameof(Models.ChatReminder)}",
            MessagePriority = MessageInfo.Success | MessageInfo.Delete,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Models.ChatReminder>()
        };

        await messageService.SendInfoMessageAsync(message);
    }
}