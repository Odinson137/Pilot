﻿using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyRoleConsumer;

public class CompanyRoleCreatedConsumer(
    ILogger<CompanyRoleCreatedConsumer> logger,
    ICompanyRole repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<CompanyRole, CompanyRoleDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}