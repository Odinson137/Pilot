using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.ChatMemberConsumer;

public class ChatMemberCreateConsumer(
    ILogger<ChatMemberCreateConsumer> logger,
    IChatMemberRepository repository,
    IBaseValidatorService validatorService,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreateConsumer<ChatMember, ChatMemberDto>(logger, repository, validatorService, mapper, notificationService);