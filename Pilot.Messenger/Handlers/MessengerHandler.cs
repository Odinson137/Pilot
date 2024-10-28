using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.Messenger.Queries;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class MessengerHandler(IMessageRepository repository, ILogger<MessengerHandler> logger)
    : ModelQueryHandler<Message, MessageDto>(repository, logger), 
        IRequestHandler<GetChatMessagesQuery, ICollection<MessageDto>>
{
    public async Task<ICollection<MessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        logger.LogClassInfo("Get chat messages");
        var result = await repository.GetMessagesAsync(request.ChatId, 0, 0, cancellationToken);
        return result;
    }
}