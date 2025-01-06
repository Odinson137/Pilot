using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class CompanyRoleController(IMediator mediator) : PilotController<CompanyRoleDto>(mediator)
{
}