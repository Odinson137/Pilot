using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Controller;
using Pilot.Storage.Queries;

namespace Pilot.Storage.Controllers;

public class FileController(IMediator mediator) : PilotReadOnlyController<FileDto>(mediator)
{
    [HttpGet(Urls.FileUrl)]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetFileUrls(CancellationToken token, [FromQuery] string? filter = null)
    {
        var baseFilter = filter?.FromJson<BaseFilter>() ?? new BaseFilter();
        var result = await Mediator.Send(new GetFileUrlsQuery<FileDto>(baseFilter), token);
        return Ok(result);
    }
    
    [HttpGet(Urls.FileUrl + "/{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public virtual async Task<IActionResult> GetFileUrl(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new GetFileUrlQuery<FileDto>(id), token);
        return Ok(result);
    }
}