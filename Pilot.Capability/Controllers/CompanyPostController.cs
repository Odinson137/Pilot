using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Capability.Queries;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Capability.Controllers;

public class CompanyPostController(IMediator mediator) : PilotReadOnlyController<CompanyPostDto>(mediator)
{
    [HttpGet(Urls.OpenCompanyPost)]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetOpenCompanyPost(CancellationToken token, [FromBody] BaseFilter? filter = null)
    {
        filter ??= new BaseFilter();
        var result = await Mediator.Send(new GetOpenCompanyPost<CompanyPostDto>(filter.Value), token);
        return Ok(result);
    }
}