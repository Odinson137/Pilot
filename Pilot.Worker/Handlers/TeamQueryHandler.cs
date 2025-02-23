using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class TeamQueryHandler : ModelQueryHandler<Team, TeamDto>
{
    public TeamQueryHandler(ITeam repository, ILogger<ModelQueryHandler<Team, TeamDto>> logger) : base(repository, logger)
    {
    }
}