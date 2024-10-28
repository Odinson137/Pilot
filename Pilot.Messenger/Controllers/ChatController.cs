using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Queries;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Messenger.Controllers;

public class ChatController(IMediator mediator) : PilotReadOnlyController<ChatDto>(mediator)
{
    [HttpGet(Urls.UserChats + "/{id:int}")]
    [ProducesResponseType(200)]
    public async Task<ICollection<ChatDto>> GetUserChats(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new GetUserChatsQuery(id), token);
        return result;
    }
}