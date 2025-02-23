using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TaskInfoConsumer;

public class TaskInfoCreatedConsumer(
    ILogger<TaskInfoCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<TaskInfo, TaskInfoDto>(logger, mediator)
{
}