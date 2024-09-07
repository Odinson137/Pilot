using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class TaskInfoHandler : ModelQueryHandler<TaskInfo, TaskInfoDto>
{
    public TaskInfoHandler(ITaskInfo repository, ILogger<TaskInfoHandler> logger) : base(repository, logger)
    {
    }
}