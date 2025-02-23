using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamConsumer;

public class TeamUpdatedConsumer(
    ILogger<TeamUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<Team, TeamDto>(logger, mediator)
{
}