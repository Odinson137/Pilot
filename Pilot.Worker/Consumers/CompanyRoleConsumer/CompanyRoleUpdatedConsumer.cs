﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Consumers.CompanyConsumer;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyRoleConsumer;

public class CompanyRoleUpdatedConsumer(
    ILogger<CompanyRoleUpdatedConsumer> logger,
    ICompanyRole repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<CompanyRole, CompanyRoleDto>(logger, repository, message, validate, mapper)
{
}