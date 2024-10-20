using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.MessageConsumer;

public class MessageCreateConsumer(
    ILogger<MessageCreateConsumer> logger,
    IMessageRepository repository,
    IBaseValidatorService validatorService,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreateConsumer<Message, MessageDto>(logger, repository, validatorService, mapper, notificationService);