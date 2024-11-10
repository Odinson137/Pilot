using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectTaskConsumer;

public class ProjectTaskCreatedConsumer(
    ILogger<ProjectTaskCreatedConsumer> logger,
    IProjectTask repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<ProjectTask, ProjectTaskDto>(logger, repository, messageService, validate, mapper,
        companyUser)
{
}