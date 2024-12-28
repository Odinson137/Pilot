using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.MessageConsumer;

public class MessageCreateConsumer(
    ILogger<MessageCreateConsumer> logger,
    IMessageRepository repository,
    IBaseValidatorService validatorService,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreateConsumer<Message, MessageDto>(logger, repository, validatorService, mapper, notificationService)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<MessageDto>> context)
    {
        Logger.LogInformation($"{nameof(Message)} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<Message, MessageDto>(context.Message.Value);

        var model = Mapper.Map<Message>(context.Message.Value);

        await Validator.FillValidateAsync(model);

        model.AddUser(context.Message.UserId);
        
        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        // var message = new InfoMessageDto
        // {
        //     Title = "Успешное создание!",
        //     Description = $"Успешное создание сущности '{nameof(MessageDto)}'",
        //     MessagePriority = MessageInfo.Success | MessageInfo.Create,
        //     EntityType = PilotEnumExtensions.GetModelEnumValue<Message>(),
        //     EntityId = model.Id
        // };
        
        // await NotificationService.SendMessage(model.Chat.Id, context.Message.Value);
    }
}