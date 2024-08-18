using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Messenger.Controllers;

public class MessageUserController(IMediator mediator) : PilotReadOnlyController<MessageDto>(mediator)
{
    [HttpPost]
    [Route("localuser")]
    [Authorize("Admin")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddLocalUser(UserDto userDto, CancellationToken token)
    {
        await Mediator.Send(userDto, token);
        return Ok();
    }
}