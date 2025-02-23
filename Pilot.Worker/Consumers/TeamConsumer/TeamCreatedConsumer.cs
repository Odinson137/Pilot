using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamConsumer;

public class TeamCreatedConsumer(
    ILogger<TeamCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<Team, TeamDto>(logger, mediator)
{
}