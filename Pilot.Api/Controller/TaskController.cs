using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Api.Handlers.TaskHandlers;

namespace Pilot.Api.Controller;

[Route("api/[controller]")]
public class TaskController : PilotController
{
    private readonly IMediator _mediator;

    public static string GetAllProjectTasksCache(string projectId) => $"tasks-{projectId}";
    // public static string GetCompanyProjectCache(string projectId) => $"project-{projectId}";

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet("/GetProjectTasks/{companyId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllTask(string companyId, string projectId, CancellationToken token)
    {
        return Ok(
            await _mediator.Send(new GetTasksQuery(companyId, projectId, GetAllProjectTasksCache(projectId)), token));
    }
}