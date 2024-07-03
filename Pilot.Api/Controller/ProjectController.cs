using MediatR;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;

namespace Pilot.Api.Controller;

public class ProjectController(IMediator mediator) : PilotController<Project, ProjectDto>(mediator);
