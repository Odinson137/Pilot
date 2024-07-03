using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Repository;

public class TeamRepository(DataContext context, IMapper mapper) : BaseRepository<Team>(context, mapper), ITeam
{

}