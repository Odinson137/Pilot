using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class GatewayController<TDto>(IMediator mediator) : PilotController<TDto>(mediator)
    where TDto : BaseDto;