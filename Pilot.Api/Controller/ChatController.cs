using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Queries;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

[Authorize]
public class ChatController(IMediator mediator) : PilotController<ChatDto>(mediator)
{
    [HttpGet(Urls.UserChats)]
    [ProducesResponseType(200)]
    public async Task<ICollection<ChatDto>> GetUserChats(CancellationToken token)
    {
        var result = await Mediator.Send(new GetUserChatsQuery(UserId), token);
        return result;
    }
}