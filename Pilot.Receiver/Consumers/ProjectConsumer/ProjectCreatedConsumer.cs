using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.ProjectConsumer;

public class ProjectCreatedConsumer(
    ILogger<ProjectCreatedConsumer> logger,
    IProject repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Project, ProjectDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}