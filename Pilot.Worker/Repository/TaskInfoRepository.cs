using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Repository;

public class TaskInfoRepository(DataContext context, IMapper mapper) : BaseRepository<TaskInfo>(context, mapper), ITaskInfo
{
}