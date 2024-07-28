using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyUserConsumer;

public class CompanyUserDeletedConsumer(
    ILogger<CompanyUserDeletedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseDeleteConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, message, validate, mapper, companyUser)
{

}