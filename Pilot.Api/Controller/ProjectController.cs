using MediatR;
using Pilot.Api.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Controller;

public class ProjectController(IMediator mediator) : GatewayController<ProjectDto>(mediator);
