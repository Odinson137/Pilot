using AutoMapper;
using MassTransit;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.BackgroundJob.Consumers.ChatReminder;

// TODO потом добавить в каждый сервис поддержку валидации на юзера, пока я сейчас слишком сильно доверяю id в токене
public class ChatReminderCreatedConsumer(
    ILogger<ChatReminderCreatedConsumer> logger,
    IChatReminder repository,
    IMessageService messageService,
    IValidatorService validate,
    IJob job,
    IMapper mapper)
    : IConsumer<CreateCommandMessage<ChatReminderDto>>
{
    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<ChatReminderDto>> context)
    {
        logger.LogInformation($"{nameof(ChatReminder)} create consume");
        logger.LogClassInfo(context.Message);

        var reminderDto = context.Message.Value;
        
        await validate.ValidateAsync<Models.ChatReminder, ChatReminderDto>(reminderDto);

        var reminder = mapper.Map<Models.ChatReminder>(reminderDto);

        await validate.FillValidateAsync(reminder);

        await repository.AddValueToContextAsync(reminder);

        await repository.SaveAsync();

        foreach (var day in reminder.DayOfWeeks)
        {
            var cronExpression = GetCronExpression(day, reminder.Time);

            // TODO потом перенести куда-то в константы. А на часах 00... поэтому мне лень)
            var jobId = $"chatReminder-{reminder.Id}-{day}";

            job.AddReminderRecurringJob(
                jobId,
                reminder.Id,
                cronExpression
            );
        }
        
        var message = new InfoMessageDto
        {
            Title = "Успешное создание ремайндера!",
            Description = $"Успешное создание сущности '{nameof(Models.ChatReminder)}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Models.ChatReminder>(),
            EntityId = reminder.Id
        };

        await messageService.SendInfoMessageAsync(message, context.Message.UserId);
    }
    
    private static string GetCronExpression(DayOfWeek day, TimeOnly time)
        => $"{time.Minute} {time.Hour} ? * {(int)day}";
}