using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.MessageConsumer;

public class MessageUpdateConsumer(
    ILogger<MessageUpdateConsumer> logger,
    IMessageRepository repository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService) : BaseUpdateConsumer<Message, MessageDto>(logger, repository, validate, mapper, notificationService)
{
}