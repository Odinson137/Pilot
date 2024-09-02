using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class ProjectTask : ModelQueryHandler<Project, ProjectDto>
{
    public ProjectTask(IProject repository, ILogger<ModelQueryHandler<Project, ProjectDto>> logger) : base(repository, logger)
    {
    }
}