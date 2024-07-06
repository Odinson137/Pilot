using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Receiver.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseSelectController<T, TDto>(IBaseReadRepository<T> repository, ILogger<BaseSelectController<T, TDto>> logger) : ControllerBase where T : BaseModel where TDto : BaseDto
{
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllValues(CancellationToken token, int skip = 0, int take = 50)
    {
        var result = await repository.GetAllValuesAsync<TDto>(skip, take, token);
        logger.LogClassInfo(result);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetValue(int id, CancellationToken token)
    {
        var result = await repository.GetByIdAsync<TDto>(id, token);
        if (result == null)
        {
            return NotFound("Пользователь не найден");
        }
        logger.LogClassInfo(result);
        return Ok(result);
    }
}