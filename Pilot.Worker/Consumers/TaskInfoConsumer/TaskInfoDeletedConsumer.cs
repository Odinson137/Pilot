using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TaskInfoConsumer;

public class TaskInfoDeletedConsumer(
    ILogger<TaskInfoDeletedConsumer> logger,
    ITaskInfo repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<TaskInfo, TaskInfoDto>(logger, repository, message, validate)
{
}