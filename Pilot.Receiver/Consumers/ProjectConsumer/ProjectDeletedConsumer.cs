using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.ProjectConsumer;

public class ProjectDeletedConsumer(
    ILogger<ProjectDeletedConsumer> logger,
    IProject repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseDeleteConsumer<Project, ProjectDto>(logger, repository, message, validate, mapper)
{

}