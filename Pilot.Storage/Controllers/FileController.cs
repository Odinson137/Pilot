using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Storage.Controllers;

public class FileController(IMediator mediator) : PilotReadOnlyController<FileDto>(mediator)
{
    [HttpGet("Url")]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetFileUrls(CancellationToken token, [FromBody] BaseFilter? filter = null)
    {
        filter ??= new BaseFilter();
        var result = await Mediator.Send(new GetValuesQuery<FileDto>(filter.Value), token);
        return Ok(result);
    }
    
    [HttpGet("Url/{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> GetFileUrl(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new GetValueByIdQuery<FileDto>(id), token);
        return Ok(result);
    }
}