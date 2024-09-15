using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.ProjectTaskConsumer;

public class ProjectTaskDeletedConsumer(
    ILogger<ProjectTaskDeletedConsumer> logger,
    IProjectTask repository,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<ProjectTask, ProjectTaskDto>(logger, repository, message, validate)
{
}