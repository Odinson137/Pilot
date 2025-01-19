using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class JobApplicationController(IMediator mediator) : PilotController<JobApplicationDto>(mediator);