using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Commands;
using Pilot.Api.Queries;

namespace Pilot.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : Microsoft.AspNetCore.Mvc.Controller
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
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddCompany(string companyName)
    {
        throw new Exception("NO USER AUTHENTIFICATION");

        await _mediator.Send(new CompanyCommand(companyName, ""));
        return Ok("The company will be adding soon");
    }
    
    [HttpPatch]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ChangeCompanyTitle(string companyId, string companyName)
    {
        throw new Exception("NO USER AUTHENTIFICATION");
        await _mediator.Send(new CompanyCommand(companyName, ""));
        return Ok("The company will be adding soon");
    }
}