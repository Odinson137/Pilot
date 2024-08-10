using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.CompanyUserConsumer;

public class CompanyUserCreatedConsumer(
    ILogger<CompanyUserCreatedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, messageService, validate, mapper, companyUser)
{
}