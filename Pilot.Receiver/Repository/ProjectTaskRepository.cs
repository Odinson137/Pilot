using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class ProjectTaskRepository(DataContext context, IMapper mapper) : BaseRepository<ProjectTask>(context, mapper), IProjectTask
{

}