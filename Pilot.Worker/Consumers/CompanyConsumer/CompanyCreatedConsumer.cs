using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Models;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    ICompany company,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Models.Company, CompanyDto>(logger, company, messageService, validate, mapper, companyUser)
{
}