using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.SqrsControllerLibrary.Controller;

[ApiController]
[Route("api/[controller]")]
public abstract class PilotController<TDto>(IMediator mediator)
    : PilotReadOnlyController<TDto>(mediator) where TDto : BaseDto
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public virtual async Task<IActionResult> AddValue(TDto valueDto, CancellationToken token)
    {
        await Mediator.Send(new AddValueCommand<TDto>(valueDto, UserId), token);
        return Ok($"The {nameof(TDto)} will be adding soon");
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> UpdateValue(TDto valueDto, CancellationToken token)
    {
        await Mediator.Send(new UpdateValueCommand<TDto>(valueDto, UserId), token);
        return Ok($"The {nameof(TDto)} will edit soon");
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> DeleteValue(int valueId, CancellationToken token) // достаточно в модели передать id
    {
        await Mediator.Send(new DeleteValueCommand<TDto>(valueId, UserId), token);
        return Ok($"The {nameof(TDto)} will delete soon");
    }
}