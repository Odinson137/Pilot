using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyUpdatedConsumer(
    ILogger<CompanyUpdatedConsumer> logger,
    ICompany company,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Models.Company, CompanyDto>(logger, company, message, validate, mapper)
{
}