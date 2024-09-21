using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamConsumer;

public class TeamDeletedConsumer(
    ILogger<TeamDeletedConsumer> logger,
    ITeam repository,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<Team, TeamDto>(logger, repository, message, validate)
{
}