using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class TaskInfoHandler : ModelQueryHandler<TaskInfo, TaskInfoDto>
{
    public TaskInfoHandler(ITaskInfo repository, ILogger<TaskInfoHandler> logger) : base(repository, logger)
    {
    }
}