using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    ICompany company,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Models.Company, CompanyDto>(logger, company, messageService, validate, mapper, companyUser)
{
}