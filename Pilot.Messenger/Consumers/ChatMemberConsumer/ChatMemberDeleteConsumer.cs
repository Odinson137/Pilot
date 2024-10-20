using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.ChatMemberConsumer;

public class ChatMemberDeleteConsumer(
    ILogger<ChatMemberDeleteConsumer> logger,
    IChatMemberRepository repository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService) : BaseDeleteConsumer<ChatMember, ChatMemberDto>(logger, repository, validate, mapper, notificationService)
{
}