using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Worker.Controllers;

public class CompanyController(IMediator mediator) : PilotReadOnlyController<CompanyDto>(mediator);