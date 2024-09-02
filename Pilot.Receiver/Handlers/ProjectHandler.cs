using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class ProjectHandler : ModelQueryHandler<Project, ProjectTaskDto>
{
    public ProjectHandler(IProject repository, ILogger<ModelQueryHandler<Project, ProjectTaskDto>> logger) : base(repository, logger)
    {
    }
}