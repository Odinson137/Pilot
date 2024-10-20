using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class MessengerHandler(IMessageRepository repository, ILogger<MessengerHandler> logger)
    : ModelQueryHandler<Message, MessageDto>(repository, logger);