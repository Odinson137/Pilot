using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.ProjectConsumer;

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