using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectCommandHandler : ModelCommandHandler<Project, ProjectDto>
{
    public ProjectCommandHandler(IProject repository, IMapper mapper, IBaseValidatorService validateService) : base(
        repository, mapper, validateService)
    {
    }
}