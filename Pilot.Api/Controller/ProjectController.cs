using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Api.DTO;
using Pilot.Api.Handlers.ProjectHandlers;

namespace Pilot.Api.Controller;


[Route("api/[controller]")]
[ApiController]
public class ProjectController : PilotController
{
    private readonly IMediator _mediator;

    public static string GetAllCompanyProjectsCache(string companyId) => $"projects-{companyId}";
    public static string GetCompanyProjectCache(string projectId) => $"project-{projectId}";

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet("/GetProjects/{companyId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllProjectsInCompany(string companyId, CancellationToken token)
    {
        var projectsDto = 
            await _mediator.Send(new GetProjectsQuery(companyId, GetAllCompanyProjectsCache(companyId)), token);
        return Ok(projectsDto);
    }
    
    [Authorize]
    [HttpGet("/GetProject/{companyId}/{projectId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetProjectInCompany(string companyId, string projectId, CancellationToken token)
    {
        var projectsDto = 
            await _mediator.Send(new GetProjectQuery(companyId, projectId, GetCompanyProjectCache(projectId)), token);
        return Ok(projectsDto);
    }
    
    [Authorize]
    [HttpPost("AddProject/{companyId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddProjectInCompany(
        CreateProjectDto createProject, CancellationToken token)
    {
        await _mediator.Send(new CreateProjectCommand(createProject), token);
        return Ok();
    }
    
    [Authorize]
    [HttpPut("UpdateProject/{companyId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ChangeProjectInCompany(
        UpdateProjectDto updateProject, CancellationToken token)
    {
        await _mediator.Send(new UpdateProjectCommand(updateProject), token);
        return Ok();
    }
}