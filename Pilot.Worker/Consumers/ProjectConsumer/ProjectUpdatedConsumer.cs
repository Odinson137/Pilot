using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectConsumer;

public class ProjectUpdatedConsumer(
    ILogger<ProjectUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<Project, ProjectDto>(logger, mediator)
{
}