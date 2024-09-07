using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Repository;

public class TaskInfoRepository(DataContext context, IMapper mapper) : BaseRepository<TaskInfo>(context, mapper), ITaskInfo
{
}