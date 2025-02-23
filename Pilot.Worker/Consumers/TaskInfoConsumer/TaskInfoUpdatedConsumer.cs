using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TaskInfoConsumer;

public class TaskInfoUpdatedConsumer(
    ILogger<TaskInfoUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<TaskInfo, TaskInfoDto>(logger, mediator)
{
}