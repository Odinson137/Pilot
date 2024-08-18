using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class ProjectTaskController(IProjectTask repository, ILogger<ProjectTaskController> logger)
    : BaseReadOnlyController<ProjectTask, ProjectTaskDto>(repository, logger);