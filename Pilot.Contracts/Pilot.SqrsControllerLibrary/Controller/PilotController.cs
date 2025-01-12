using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.SqrsControllerLibrary.Controller;

public abstract class PilotController<TDto>(IMediator mediator)
    : PilotReadOnlyController<TDto>(mediator) where TDto : BaseDto
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public virtual async Task<IActionResult> AddValue([FromBody] TDto valueDto, CancellationToken token)
    {
        await Mediator.Send(new CreateCommand<TDto>(valueDto, UserId), token);
        return Ok($"The {typeof(TDto).Name} will be adding soon");
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> UpdateValue(TDto valueDto, CancellationToken token)
    {
        await Mediator.Send(new UpdateCommand<TDto>(valueDto, UserId), token);
        return Ok($"The {typeof(TDto).Name} will edit soon");
    }

    [HttpDelete("{valueId:int}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> DeleteValue(int valueId, CancellationToken token)
    {
        await Mediator.Send(new DeleteCommand<TDto>(valueId, UserId), token);
        return Ok($"The {typeof(TDto).Name} will delete soon");
    }
}