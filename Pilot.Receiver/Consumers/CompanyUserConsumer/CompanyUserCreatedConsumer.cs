using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyUserConsumer;

public class CompanyUserCreatedConsumer(
    ILogger<CompanyUserCreatedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, messageService, validate, mapper, companyUser)
{
}