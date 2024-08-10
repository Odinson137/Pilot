using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class ProjectController(IProject repository, ILogger<ProjectController> logger) : BaseSelectController<Project, ProjectDto>(repository, logger);