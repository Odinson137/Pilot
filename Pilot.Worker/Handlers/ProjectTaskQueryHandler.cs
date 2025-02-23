using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectTaskQueryHandler : ModelQueryHandler<ProjectTask, ProjectTaskDto>
{
    public ProjectTaskQueryHandler(IProjectTask repository, ILogger<ModelQueryHandler<ProjectTask, ProjectTaskDto>> logger) : base(repository, logger)
    {
    }
}