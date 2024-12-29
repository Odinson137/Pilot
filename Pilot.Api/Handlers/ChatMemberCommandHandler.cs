using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class ChatMemberCommandHandler(IBaseMassTransitService massTransitService)
    : ModelCommandHandler<ChatMemberDto>(massTransitService);