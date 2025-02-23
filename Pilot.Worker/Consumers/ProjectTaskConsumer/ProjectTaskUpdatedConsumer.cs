using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectTaskConsumer;

public class ProjectTaskUpdatedConsumer(
    ILogger<ProjectTaskUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<ProjectTask, ProjectTaskDto>(logger, mediator)
{
}