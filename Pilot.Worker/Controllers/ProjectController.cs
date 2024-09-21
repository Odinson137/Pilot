using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Worker.Controllers;

public class ProjectController(IMediator mediator)
    : PilotReadOnlyController<ProjectDto>(mediator);