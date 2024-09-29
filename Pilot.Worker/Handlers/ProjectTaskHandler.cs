using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectTaskHandler : ModelQueryHandler<ProjectTask, ProjectTaskDto>
{
    public ProjectTaskHandler(IProjectTask repository, ILogger<ModelQueryHandler<ProjectTask, ProjectTaskDto>> logger) : base(repository, logger)
    {
    }
}