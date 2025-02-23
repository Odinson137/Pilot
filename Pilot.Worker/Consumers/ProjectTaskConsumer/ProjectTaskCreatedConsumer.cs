using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectTaskConsumer;

public class ProjectTaskCreatedConsumer(
    ILogger<ProjectTaskCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<ProjectTask, ProjectTaskDto>(logger, mediator)
{
}