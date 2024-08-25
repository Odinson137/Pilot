using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.TeamConsumer;

public class TeamDeletedConsumer(
    ILogger<TeamDeletedConsumer> logger,
    ITeam repository,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<Team, TeamDto>(logger, repository, message, validate)
{
}