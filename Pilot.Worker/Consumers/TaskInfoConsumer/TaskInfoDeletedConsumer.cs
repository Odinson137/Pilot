using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TaskInfoConsumer;

public class TaskInfoDeletedConsumer(
    ILogger<TaskInfoDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<TaskInfo, TaskInfoDto>(logger, mediator)
{
}