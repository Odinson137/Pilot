using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyUserConsumer;

public class CompanyUserUpdatedConsumer(
    ILogger<CompanyUserUpdatedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, message, validate, mapper)
{
}