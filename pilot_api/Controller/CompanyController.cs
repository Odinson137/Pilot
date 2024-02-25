using MediatR;
using Microsoft.AspNetCore.Mvc;
using pilot_api.Commands;
using pilot_api.Queries;
using ILogger = Serilog.ILogger;

namespace pilot_api.Controller;

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
        var result = await _mediator.Send(new AddCompanyCommand(companyName));
        return Ok(result);
    }
}