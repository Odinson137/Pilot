using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Repository;

public class TeamRepository(DataContext context, ReadOnlyDataContext readOnlyDataContext, IMapper mapper)
    : BaseReadWriteSplitRepository<Team>(context, readOnlyDataContext, mapper), ITeam
{
}