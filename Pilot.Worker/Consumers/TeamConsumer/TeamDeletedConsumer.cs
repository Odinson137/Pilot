using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamConsumer;

public class TeamDeletedConsumer(
    ILogger<TeamDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<Team, TeamDto>(logger, mediator)
{
}