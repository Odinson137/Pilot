using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class TeamEmployeeController(IMediator mediator) : PilotController<TeamEmployeeDto>(mediator);