using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Worker.Controllers;

public class CompanyRoleController(IMediator mediator) : PilotReadOnlyController<CompanyRoleDto>(mediator);