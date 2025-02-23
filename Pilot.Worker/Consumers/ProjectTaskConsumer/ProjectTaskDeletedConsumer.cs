using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectTaskConsumer;

public class ProjectTaskDeletedConsumer(
    ILogger<ProjectTaskDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<ProjectTask, ProjectTaskDto>(logger, mediator)
{
}