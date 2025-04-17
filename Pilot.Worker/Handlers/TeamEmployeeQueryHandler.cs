using Pilot.Worker.Interface;
using Pilot.Worker.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Worker.Handlers;

public class TeamEmployeeQueryHandler(ITeamEmployee repository, ILogger<TeamEmployeeQueryHandler> logger)
    : ModelQueryHandler<TeamEmployee, TeamEmployeeDto>(repository, logger);