using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Repository;

public class ProjectTaskRepository(DataContext context, IMapper mapper)
    : BaseRepository<ProjectTask>(context, mapper), IProjectTask
{
}