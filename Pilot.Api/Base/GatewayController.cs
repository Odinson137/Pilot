using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.SqrsController.Controller;

namespace Pilot.Api.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class GatewayController<T, TDto>(IMediator mediator) : PilotController<T, TDto>(mediator)
    where TDto : BaseDto;
