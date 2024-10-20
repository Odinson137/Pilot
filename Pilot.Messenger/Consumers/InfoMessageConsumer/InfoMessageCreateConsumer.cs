using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.InfoMessageConsumer;

public class InfoMessageCreateConsumer(
    ILogger<InfoMessageCreateConsumer> logger,
    IInfoMessageRepository messageRepository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreateConsumer<InfoMessage, InfoMessageDto>(logger, messageRepository, validate, mapper, notificationService)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<InfoMessageDto>> context)
    {
        Logger.LogInformation("Info message create consume");
        Logger.LogClassInfo(context.Message);

        var message = context.Message.Value;
        var userId = context.Message.UserId;

        // await Validator.ValidateAsync<InfoMessage, InfoMessageDto>(message); // info message будет создаваться только на сервере, поэтому валидацию делать, думаю, не нужно

        var model = Mapper.Map<InfoMessage>(context.Message.Value);

        model.UserId = userId;

        await Validator.FillValidateAsync(model);

        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        await NotificationService.Notify(userId, message);

        Logger.LogInformation("Message consumed");
    }
}