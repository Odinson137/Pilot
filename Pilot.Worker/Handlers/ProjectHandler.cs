using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectHandler : ModelQueryHandler<Project, ProjectDto>
{
    public ProjectHandler(IProject repository, ILogger<ModelQueryHandler<Project, ProjectDto>> logger) : base(repository, logger)
    {
    }
}