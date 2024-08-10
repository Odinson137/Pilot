using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAllValues(CancellationToken token, int skip = 0, int take = 50)
    {
        var result = await _user.GetAllValuesAsync<UserDto>(skip, take, token);
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