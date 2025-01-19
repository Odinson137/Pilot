using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Capability.Controllers;

public class JobApplicationController(IMediator mediator) : PilotReadOnlyController<JobApplicationDto>(mediator);