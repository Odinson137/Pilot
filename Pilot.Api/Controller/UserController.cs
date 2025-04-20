using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Api.Queries;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class UserController : PilotController<UserDto>
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Registration")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegistrationUser(RegistrationUserDto userDto, CancellationToken token)
    {
        await _mediator.Send(new UserRegistrationCommand(userDto), token);
        return Ok();
    }

    [HttpPost("Authorization")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AuthorizationUser(AuthorizationUserDto userDto, CancellationToken token)
    {
        var content =
            await _mediator.Send(new UserAuthorizationCommand(userDto), token);
        return Ok(content);
    }
    
    [HttpGet]
    [Route(Urls.CurrentUser)]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken token)
    {
        var content =
            await _mediator.Send(new GetUserQuery(UserId), token);
        return Ok(content);
    }
}