using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Api.Handlers.CompanyUserHandlers;

namespace Pilot.Api.Controller;


[Route("api/[controller]")]
[ApiController]
public class CompanyUserController : PilotController
{
    private readonly IMediator _mediator;
    public static string GetAllCompanyUsersCache(string companyId) => $"company-users-{companyId}";

    public CompanyUserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet("{companyId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllUserInCompany(string companyId, CancellationToken token)
    {
        var companyUsersDto = 
            await _mediator.Send(new GetCompanyUsersQuery(companyId, GetAllCompanyUsersCache(companyId)), token);
        return Ok(companyUsersDto);
    }
    
    [Authorize]
    [HttpPost("addUser")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddNewUserToCompany(string addedUserId, string companyId, CancellationToken token)
    {
        await _mediator.Send(
            new AddCompanyUserCommand(addedUserId, UserId, companyId), token);
        return Ok();
    }
}