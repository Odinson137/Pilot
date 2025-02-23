using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamEmployeeConsumer;

public class TeamEmployeeCreatedConsumer(
    ILogger<TeamEmployeeCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<TeamEmployee, TeamEmployeeDto>(logger, mediator)
{
}