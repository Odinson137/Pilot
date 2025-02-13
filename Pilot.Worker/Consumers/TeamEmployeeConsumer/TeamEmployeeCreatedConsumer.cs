using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamEmployeeConsumer;

public class TeamEmployeeCreatedConsumer(
    ILogger<TeamEmployeeCreatedConsumer> logger,
    ITeamEmployee repository,
    ICompanyUser companyUser,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<TeamEmployee, TeamEmployeeDto>(logger, repository, messageService, validate, mapper,
        companyUser)
{
}