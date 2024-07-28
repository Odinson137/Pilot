using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyConsumer;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    ICompany company,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Company, CompanyDto>(logger, company, messageService, validate, mapper, companyUser)
{
}