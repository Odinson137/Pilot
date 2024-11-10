using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Controller;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Api.Controller;

public class CompanyPostController(IMediator mediator) : PilotController<CompanyPostDto>(mediator)
{
    [HttpGet(Urls.OpenCompanyPost)]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetOpenCompanyPost(CancellationToken token, [FromQuery] string? filter = null)
    {
        var baseFilter = filter?.FromJson<BaseFilter>() ?? new BaseFilter();
        var result = await Mediator.Send(new GetValuesQuery<CompanyPostDto>(baseFilter, Urls.OpenCompanyPost), token);
        return Ok(result);
    }
}