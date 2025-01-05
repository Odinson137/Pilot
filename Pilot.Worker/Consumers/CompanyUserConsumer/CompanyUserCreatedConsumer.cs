using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyUserConsumer;

public class CompanyUserCreatedConsumer(
    ILogger<CompanyUserCreatedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, messageService, validate, mapper,
        companyUser)
{
}