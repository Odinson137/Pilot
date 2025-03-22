using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectTaskQueryHandler(
    IProjectTask repository,
    ILogger<ProjectTaskQueryHandler> logger)
    : ModelQueryHandler<ProjectTask, ProjectTaskDto>(repository, logger);