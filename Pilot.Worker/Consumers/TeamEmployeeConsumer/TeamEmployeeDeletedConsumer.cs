using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamEmployeeConsumer;

public class TeamEmployeeDeletedConsumer(
    ILogger<TeamEmployeeDeletedConsumer> logger,
    ITeamEmployee repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<TeamEmployee, TeamEmployeeDto>(logger, repository, message, validate);