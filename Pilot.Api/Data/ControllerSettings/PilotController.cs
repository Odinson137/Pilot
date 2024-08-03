using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;

namespace Pilot.Api.Data.ControllerSettings;

[ApiController]
[Route("api/[controller]")]
public abstract class PilotController<T, TDto>(IMediator mediator) : ControllerBase where TDto : BaseDto
{
    protected readonly IMediator Mediator = mediator;
    protected int UserId => int.Parse(User.Identities.First().Claims.First().Value);

    [HttpHead]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetAllValuesCount(CancellationToken token)
    {
        var result = await Mediator.Send(new GetValueByIdQuery<TDto>(1), token);
        return Ok(1);
    }
    
    [HttpGet]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetAllValues(CancellationToken token, int skip = 0, int take = 50)
    {
        var result = await Mediator.Send(new GetValuesQuery<TDto>(skip, take), token);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> GetValue(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new GetValueByIdQuery<TDto>(id), token);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public virtual async Task<IActionResult> AddValue(TDto valueDto, CancellationToken token)
    {
        await Mediator.Send(new AddValueCommand<TDto>(valueDto, UserId), token);
        return Ok($"The {nameof(T)} will be adding soon");
    }
    
    [HttpPut]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> UpdateValue(TDto valueDto)
    {
        await Mediator.Send(new UpdateValueCommand<TDto>(valueDto, UserId));
        return Ok($"The {nameof(T)} will edit soon");
    }
    
    [HttpDelete]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> DeleteValue(TDto valueDto) // достаточно в модели передать id
    {
        await Mediator.Send(new DeleteValueCommand<TDto>(valueDto, UserId));
        return Ok($"The {nameof(T)} will delete soon");
    }
}