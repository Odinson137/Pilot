using AutoMapper;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;
using Pilot.Contracts.Base;

namespace Pilot.Worker.Repository;

public class TeamEmployeeRepository(DataContext context, IMapper mapper) : BaseRepository<TeamEmployee>(context, mapper), ITeamEmployee
{

}