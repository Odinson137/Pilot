using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamConsumer;

public class TeamCreatedConsumer(
    ILogger<TeamCreatedConsumer> logger,
    ITeam repository,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Team, TeamDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}