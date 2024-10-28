using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.Messenger.Queries;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class ChatHandler(IChatRepository repository, ILogger<ChatHandler> logger)
    : ModelQueryHandler<Chat, ChatDto>(repository, logger), 
        IRequestHandler<GetUserChatsQuery, ICollection<ChatDto>>
{
    public async Task<ICollection<ChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        Logger.LogClassInfo("Get user chats");
        var result = await repository.GetUserChatsAsync(request.UserId, cancellationToken);
        return result;
    }
}