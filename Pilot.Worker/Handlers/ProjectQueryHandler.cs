using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectQueryHandler : ModelQueryHandler<Project, ProjectDto>
{
    public ProjectQueryHandler(IProject repository, ILogger<ModelQueryHandler<Project, ProjectDto>> logger) : base(repository, logger)
    {
    }
}