using MediatR;
using Pilot.Api.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;

namespace Pilot.Api.Controller;

public class ProjectTaskController(IMediator mediator) : GatewayController<ProjectTask, ProjectTaskDto>(mediator);
