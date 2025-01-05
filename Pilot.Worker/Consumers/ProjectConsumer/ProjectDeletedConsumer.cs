using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectConsumer;

public class ProjectDeletedConsumer(
    ILogger<ProjectDeletedConsumer> logger,
    IProject repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<Project, ProjectDto>(logger, repository, message, validate)
{
}