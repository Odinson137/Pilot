using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class ChatMemberHandler(IChatMemberRepository repository, ILogger<ChatMemberHandler> logger)
    : ModelQueryHandler<ChatMember, ChatMemberDto>(repository, logger);