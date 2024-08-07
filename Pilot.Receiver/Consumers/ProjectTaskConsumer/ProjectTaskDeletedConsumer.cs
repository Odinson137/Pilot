﻿using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.ProjectTaskConsumer;

public class ProjectTaskDeletedConsumer(
    ILogger<ProjectTaskDeletedConsumer> logger,
    IProjectTask repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseDeleteConsumer<ProjectTask, ProjectTaskDto>(logger, repository, message, validate, mapper)
{

}