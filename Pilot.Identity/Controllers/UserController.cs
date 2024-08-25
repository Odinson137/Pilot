using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUser _user;

    public UserController(IUser user)
    {
        _user = user;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllValues([FromBody] BaseFilter filter, CancellationToken token)
    {
        var result = await _user.GetValuesAsync<UserDto>(filter, token);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetValue(int id, CancellationToken token)
    {
        var result = await _user.GetByIdAsync<UserDto>(id, token);
        return Ok(result);
    }
}