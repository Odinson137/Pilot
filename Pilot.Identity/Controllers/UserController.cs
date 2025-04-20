using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Identity.Controllers;

public class UserController(IMediator mediator) : PilotController<UserDto>(mediator)
{
}