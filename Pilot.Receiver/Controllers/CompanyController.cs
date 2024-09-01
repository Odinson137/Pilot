using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Receiver.Controllers;

public class CompanyController(IMediator mediator) : PilotReadOnlyController<CompanyDto>(mediator);