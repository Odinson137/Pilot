using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Api.Queries;

namespace Pilot.Api.Data.ControllerSettings;

public abstract class PilotController<T, TDto>(IMediator mediator) : ControllerBase
{
    protected string UserId => User.Identities.First().Claims.First().Value;

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllValues(CancellationToken token, int skip = 0, int take = 50)
    {
        var result = await mediator.Send(new GetValuesQuery<T>(skip, take, $"all-{nameof(T)}-{skip}-{take}"), token);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetValue(int id, CancellationToken token)
    {
        var result = await mediator.Send(new GetValueByIdQuery<T>(id, $"{nameof(T)}Id-{id}"), token);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddValue(TDto valueDto, CancellationToken token)
    {
        await mediator.Send(new AddValueCommand<TDto>(valueDto, UserId), token);
        return Ok($"The {nameof(T)} will be adding soon");
    }
    
    [HttpPut]
    [Authorize]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateValue(TDto valueDto)
    {
        await mediator.Send(new UpdateValueCommand<TDto>(valueDto, UserId));
        return Ok($"The {nameof(T)} will be editing soon");
    }
}