using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.ChatConsumer;

public class ChatCreateConsumer(
    ILogger<BaseCreateConsumer<Chat, ChatDto>> logger,
    IChatRepository repository,
    IBaseValidatorService validatorService,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreateConsumer<Chat, ChatDto>(logger, repository, validatorService, mapper, notificationService)
{
    
}