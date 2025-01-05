using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.BackgroundJob.Consumers.ChatReminder;

public class ChatReminderUpdateConsumer(
    ILogger<ChatReminderUpdateConsumer> logger,
    IChatReminder repository,
    IMessageService messageService,
    IJob job,
    IValidatorService validate,
    IMapper mapper)
    : IConsumer<UpdateCommandMessage<ChatReminderDto>>
{

    public virtual async Task Consume(ConsumeContext<UpdateCommandMessage<ChatReminderDto>> context)
    {
        logger.LogInformation($"{nameof(Models.ChatReminder)} update consume");

        var dtoModel = context.Message.Value;

        await validate.ValidateAsync<Models.ChatReminder, ChatReminderDto>(dtoModel);

        var model = mapper.Map<Models.ChatReminder>(dtoModel);

        await validate.FillValidateAsync(model);
        
        model.ChangeAt = DateTime.Now;

        repository.GetContext.Attach(model);
        repository.GetContext.Entry(model).State = EntityState.Modified;

        foreach (var dayOfWeek in Enum.GetValues<DayOfWeek>())
        {
            var jobId = $"chatReminder-{model.Id}-{dayOfWeek}";
            job.RemoveReminderRecurringJob(jobId);
        }
        
        foreach (var day in model.DayOfWeeks)
        {
            var time = model.Time.AddHours(3); // TODO про это не забыть
            var cronExpression = GetCronExpression(day, time);
            var jobId = $"chatReminder-{model.Id}-{day}";
            job.AddReminderRecurringJob(
                jobId,
                model.Id,
                cronExpression
            );
        }
        
        await repository.SaveAsync();

        var message = new InfoMessageDto
        {
            Title = "Успешное обновление!",
            Description = $"Успешное обновление сущности {nameof(Models.ChatReminder)}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Update,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Models.ChatReminder>(),
            EntityId = model.Id
        };

        await messageService.SendInfoMessageAsync(message, context.Message.UserId);
    }
    
    private static string GetCronExpression(DayOfWeek day, TimeOnly time)
        => $"{time.Minute} {time.Hour} ? * {(int)day}";
}