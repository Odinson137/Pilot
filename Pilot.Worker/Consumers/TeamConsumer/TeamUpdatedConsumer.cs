using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamConsumer;

public class TeamUpdatedConsumer(
    ILogger<TeamUpdatedConsumer> logger,
    ITeam repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Team, TeamDto>(logger, repository, message, validate, mapper)
{
}