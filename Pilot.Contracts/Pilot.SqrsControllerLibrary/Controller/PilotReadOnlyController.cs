﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.SqrsControllerLibrary.Controller;

[ApiController]
[Route("api/[controller]")]
public abstract class PilotReadOnlyController<TDto>(IMediator mediator) : BaseController where TDto : BaseDto
{
    protected readonly IMediator Mediator = mediator;

    [HttpGet]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetAllValues(CancellationToken token, BaseFilter? filter = null)
    {
        filter ??= new BaseFilter();
        var result = await Mediator.Send(new GetValuesQuery<TDto>(filter.Value), token);
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
}