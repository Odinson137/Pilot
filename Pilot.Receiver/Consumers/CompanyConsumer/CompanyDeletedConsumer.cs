using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyConsumer;

public class CompanyDeletedConsumer(
    ILogger<CompanyDeletedConsumer> logger,
    ICompany company,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseDeleteConsumer<Company, CompanyDto>(logger, company, message, validate, mapper)
{

}