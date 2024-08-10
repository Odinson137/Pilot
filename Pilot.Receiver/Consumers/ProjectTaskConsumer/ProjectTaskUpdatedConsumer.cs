using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.ProjectTaskConsumer;

public class ProjectTaskUpdatedConsumer(
    ILogger<ProjectTaskUpdatedConsumer> logger,
    IProjectTask repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<ProjectTask, ProjectTaskDto>(logger, repository, message, validate, mapper)
{
}