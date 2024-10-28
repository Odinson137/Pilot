using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Queries;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

[Authorize]
public class MessageController(IMediator mediator) : PilotController<InfoMessageDto>(mediator)
{
    [HttpGet(Urls.ChatMessages + "/{chatId:int}")]
    [ProducesResponseType(200)]
    public async Task<ICollection<MessageDto>> GetUserChats(int chatId, CancellationToken token)
    {
        var result = await Mediator.Send(new GetChatMessagesQuery(chatId), token);
        return result;
    }
}