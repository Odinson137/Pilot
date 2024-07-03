using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Controllers;

public class ProjectTaskController(IProjectTask repository, ILogger<ProjectTaskController> logger) : BaseSelectController<ProjectTask, ProjectTaskDto>(repository, logger);