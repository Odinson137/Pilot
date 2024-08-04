using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.TeamConsumer;

public class TeamDeletedConsumer(
    ILogger<TeamDeletedConsumer> logger,
    ITeam repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseDeleteConsumer<Team, TeamDto>(logger, repository, message, validate, mapper, companyUser)
{

}