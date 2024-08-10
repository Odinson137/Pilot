using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interface;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Consumers.MessageConsumer;

public class MessageCreatedConsumer(
    ILogger<MessageCreatedConsumer> logger,
    IMessageRepository company,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<Message, MessageDto>(logger, company, validate, mapper)
{
}