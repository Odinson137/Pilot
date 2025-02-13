using Pilot.Worker.Interface;
using Pilot.Worker.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Worker.Handlers;

public class Handler : ModelQueryHandler<TeamEmployee, TeamEmployeeDto>
{
    public Handler(ITeamEmployee repository, ILogger<Handler> logger) : base(repository, logger)
    {
    }
}