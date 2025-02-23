using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectConsumer;

public class ProjectCreatedConsumer(
    ILogger<ProjectCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<Project, ProjectDto>(logger, mediator)
{
}