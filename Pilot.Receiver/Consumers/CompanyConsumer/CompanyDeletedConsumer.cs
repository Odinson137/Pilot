using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyConsumer;

public class CompanyDeletedConsumer(
    ILogger<CompanyDeletedConsumer> logger,
    ICompany company,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseDeleteConsumer<Company, CompanyDto>(logger, company, message, validate, mapper)
{

}