using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
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
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetUser(CancellationToken token)
    {
        var content =
            await _mediator.Send(new GetUserQuery(UserId), token);
        return Ok(content);
    }
}