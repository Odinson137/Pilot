using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUser _user;
    private readonly ILogger<UserController> _logger;

    public UserController(IUser user, ILogger<UserController> logger)
    {
        _user = user;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllValues(CancellationToken token, [FromQuery] string? filter = null)
    {
        _logger.LogInformation(filter);
        var baseFilter = filter?.FromJson<BaseFilter>() ?? new BaseFilter();
        var result = await _user.GetValuesAsync<UserDto>(baseFilter, token);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetValue(int id, CancellationToken token)
    {
        _logger.LogInformation("Get user by - " + id);
        var result = await _user.GetByIdAsync<UserDto>(id, token);
        return Ok(result);
    }
}