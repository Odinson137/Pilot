using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.ProjectConsumer;

public class ProjectCreatedConsumer(
    ILogger<ProjectCreatedConsumer> logger,
    IProject repository,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Project, ProjectDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}