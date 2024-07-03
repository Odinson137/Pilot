using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Controllers;

public class TeamController(ITeam repository, ILogger<TeamController> logger) : BaseSelectController<Team, TeamDto>(repository, logger);