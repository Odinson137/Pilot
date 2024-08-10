using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Repository;

public class TeamRepository(DataContext context, IMapper mapper) : BaseRepository<Team>(context, mapper), ITeam
{

}