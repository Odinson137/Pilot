using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.ProjectConsumer;

public class ProjectUpdatedConsumer(
    ILogger<ProjectUpdatedConsumer> logger,
    IProject repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Project, ProjectDto>(logger, repository, message, validate, mapper)
{
}