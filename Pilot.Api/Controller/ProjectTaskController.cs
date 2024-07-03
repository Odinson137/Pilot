using MediatR;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;

namespace Pilot.Api.Controller;

public class ProjectTaskController(IMediator mediator) : PilotController<ProjectTask, ProjectTaskDto>(mediator);
