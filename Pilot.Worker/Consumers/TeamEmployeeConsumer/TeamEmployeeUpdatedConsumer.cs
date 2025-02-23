using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamEmployeeConsumer;

public class TeamEmployeeUpdatedConsumer(
    ILogger<TeamEmployeeUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<TeamEmployee, TeamEmployeeDto>(logger, mediator)
{
}