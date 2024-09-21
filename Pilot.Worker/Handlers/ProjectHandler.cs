using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectHandler : ModelQueryHandler<Project, ProjectTaskDto>
{
    public ProjectHandler(IProject repository, ILogger<ModelQueryHandler<Project, ProjectTaskDto>> logger) : base(repository, logger)
    {
    }
}