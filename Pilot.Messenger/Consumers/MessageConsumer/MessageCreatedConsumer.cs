using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.MessageConsumer;

public class MessageCreatedConsumer(
    ILogger<MessageCreatedConsumer> logger,
    IMessageRepository messageRepository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreatedConsumer<Message, MessageDto>(logger, messageRepository, validate, mapper, notificationService)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<MessageDto>> context)
    {
        Logger.LogInformation("Message create consume");
        Logger.LogClassInfo(context.Message);

        var message = context.Message.Value;
        var userId = context.Message.UserId;

        await Validator.ValidateAsync<Message, MessageDto>(message, userId);

        var model = Mapper.Map<Message>(context.Message.Value);

        model.UserId = userId;

        await Validator.FillValidateAsync(model);

        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        await NotificationService.Notify(userId, message);

        Logger.LogInformation("Message consumed");
    }
}