using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class TeamHandler : ModelQueryHandler<Team, TeamDto>
{
    public TeamHandler(ITeam repository, ILogger<ModelQueryHandler<Team, TeamDto>> logger) : base(repository, logger)
    {
    }
}