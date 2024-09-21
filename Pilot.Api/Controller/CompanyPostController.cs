using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Api.Controller;

public class CompanyPostController(IMediator mediator) : PilotReadOnlyController<CompanyPostDto>(mediator)
{
    [HttpGet(Urls.OpenCompanyPost)]
    [ProducesResponseType(200)]
    public virtual async Task<IActionResult> GetOpenCompanyPost(CancellationToken token, [FromBody] BaseFilter? filter = null)
    {
        filter ??= new BaseFilter();
        var result = await Mediator.Send(new GetValuesQuery<CompanyPostDto>(filter.Value, Urls.OpenCompanyPost), token);
        return Ok(result);
    }
}