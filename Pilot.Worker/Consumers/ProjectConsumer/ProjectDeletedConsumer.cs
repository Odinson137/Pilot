using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectConsumer;

public class ProjectDeletedConsumer(
    ILogger<ProjectDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<Project, ProjectDto>(logger, mediator)
{
}