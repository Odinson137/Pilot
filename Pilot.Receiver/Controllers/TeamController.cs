using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class TeamController(ITeam repository, ILogger<TeamController> logger) : BaseSelectController<Team, TeamDto>(repository, logger);