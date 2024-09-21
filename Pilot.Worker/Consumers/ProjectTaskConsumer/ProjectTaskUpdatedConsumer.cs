using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectTaskConsumer;

public class ProjectTaskUpdatedConsumer(
    ILogger<ProjectTaskUpdatedConsumer> logger,
    IProjectTask repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<ProjectTask, ProjectTaskDto>(logger, repository, message, validate, mapper)
{
}