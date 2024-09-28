using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectTaskConsumer;

public class ProjectTaskDeletedConsumer(
    ILogger<ProjectTaskDeletedConsumer> logger,
    IProjectTask repository,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<ProjectTask, ProjectTaskDto>(logger, repository, message, validate)
{
}