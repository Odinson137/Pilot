using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Queries;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Messenger.Controllers;

public class MessageController(IMediator mediator) : PilotReadOnlyController<MessageDto>(mediator)
{
    [HttpGet(Urls.ChatMessages + "/{chatId:int}")]
    [ProducesResponseType(200)]
    public async Task<ICollection<MessageDto>> GetChatMessage(int chatId, CancellationToken token)
    {
        var result = await Mediator.Send(new GetChatMessagesQuery(chatId), token);
        return result;
    }
}