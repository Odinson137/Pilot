using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class ChatHandler(IChatRepository repository, ILogger<ChatHandler> logger)
    : ModelQueryHandler<Chat, ChatDto>(repository, logger);