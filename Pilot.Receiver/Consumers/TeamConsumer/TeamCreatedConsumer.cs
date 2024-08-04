using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.TeamConsumer;

public class TeamCreatedConsumer(
    ILogger<TeamCreatedConsumer> logger,
    ITeam repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Team, TeamDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}