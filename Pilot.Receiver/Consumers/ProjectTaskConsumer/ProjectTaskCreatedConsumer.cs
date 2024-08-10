using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.ProjectTaskConsumer;

public class ProjectTaskCreatedConsumer(
    ILogger<ProjectTaskCreatedConsumer> logger,
    IProjectTask repository,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<ProjectTask, ProjectTaskDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}