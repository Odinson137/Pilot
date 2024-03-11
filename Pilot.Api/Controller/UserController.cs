using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Api.DTO;
using Pilot.Receiver.DTO;

namespace Pilot.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
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
        await _mediator.Send(new UserRegistrationCommand(
                userDto.UserName, userDto.Name, userDto.LastName, userDto.Password), token);
        return Ok();
    }
    
    [HttpPost("Authorization")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AuthorizationUser(AuthorizationUserDto userDto, CancellationToken token)
    {
        var content = 
            await _mediator.Send(new UserAuthorizationCommand(userDto.UserName, userDto.Password), token);
        return Ok(content);
    }
}