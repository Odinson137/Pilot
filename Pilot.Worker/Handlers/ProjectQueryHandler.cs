using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectQueryHandler(
    IProject repository, 
    ILogger<ProjectQueryHandler> logger)
    : ModelQueryHandler<Project, ProjectDto>(repository, logger);