using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectTask : ModelQueryHandler<Project, ProjectDto>
{
    public ProjectTask(IProject repository, ILogger<ModelQueryHandler<Project, ProjectDto>> logger) : base(repository, logger)
    {
    }
}