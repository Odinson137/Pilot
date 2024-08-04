using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.TeamConsumer;

public class TeamUpdatedConsumer(
    ILogger<TeamUpdatedConsumer> logger,
    ITeam repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Team, TeamDto>(logger, repository, message, validate, mapper)
{
}