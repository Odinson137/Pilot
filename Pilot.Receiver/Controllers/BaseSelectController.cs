using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Receiver.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseReadOnlyController<T, TDto>(
    IBaseReadRepository<T> repository,
    ILogger<BaseReadOnlyController<T, TDto>> logger) : ControllerBase where T : BaseModel where TDto : BaseDto
{
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllValues(BaseFilter filter, CancellationToken token)
    {
        var result = await repository.GetValuesAsync<TDto>(filter, token);
        logger.LogClassInfo(result);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetValue(int id, CancellationToken token)
    {
        var result = await repository.GetByIdAsync<TDto>(id, token);
        if (result == null) return NotFound($"{typeof(TDto).Namespace} not found");
        logger.LogClassInfo(result);
        return Ok(result);
    }
}