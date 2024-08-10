using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.ProjectTaskConsumer;

public class ProjectTaskCreatedConsumer(
    ILogger<ProjectTaskCreatedConsumer> logger,
    IProjectTask repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<ProjectTask, ProjectTaskDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}