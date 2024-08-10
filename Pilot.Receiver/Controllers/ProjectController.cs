using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Controllers;

public class ProjectController(IProject repository, ILogger<ProjectController> logger) : BaseSelectController<Project, ProjectDto>(repository, logger);