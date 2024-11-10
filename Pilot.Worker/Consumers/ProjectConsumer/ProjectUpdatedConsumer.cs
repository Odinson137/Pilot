using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectConsumer;

public class ProjectUpdatedConsumer(
    ILogger<ProjectUpdatedConsumer> logger,
    IProject repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Project, ProjectDto>(logger, repository, message, validate, mapper)
{
}