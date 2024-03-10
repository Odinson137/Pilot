using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Api.Queries;

namespace Pilot.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : PilotController
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllCompany()
    {
        var result = await _mediator.Send(new GetCompaniesQuery());
        return Ok(result);
    }
    
    [HttpGet("{companyId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetCompany(string companyId)
    {
        var result = await _mediator.Send(new GetCompanyByIdQuery(companyId));
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddCompany(string companyName)
    {
        await _mediator.Send(new CompanyAddCommand(companyName, UserId));
        return Ok("The company will be adding soon");
    }
    
    // [HttpPatch]
    // [Authorize]
    // [ProducesResponseType(200)]
    // public async Task<IActionResult> ChangeCompanyTitle(string companyId, string companyName)
    // {
    //     await _mediator.Send(new ChangeCompanyTitleCommand(companyId, companyName, UserId));
    //     return Ok("The company will be adding soon");
    // }
}