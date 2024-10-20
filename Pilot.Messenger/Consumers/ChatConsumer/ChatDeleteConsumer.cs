using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.ChatConsumer;

public class ChatDeleteConsumer(
    ILogger<ChatDeleteConsumer> logger,
    IChatRepository repository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService) : BaseDeleteConsumer<Chat, ChatDto>(logger, repository, validate, mapper, notificationService)
{
}