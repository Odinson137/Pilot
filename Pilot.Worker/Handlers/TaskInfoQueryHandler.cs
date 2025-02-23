using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class TaskInfoQueryHandler : ModelQueryHandler<TaskInfo, TaskInfoDto>
{
    public TaskInfoQueryHandler(ITaskInfo repository, ILogger<TaskInfoQueryHandler> logger) : base(repository, logger)
    {
    }
}