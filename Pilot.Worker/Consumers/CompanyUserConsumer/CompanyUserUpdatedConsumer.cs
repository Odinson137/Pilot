using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyUserConsumer;

public class CompanyUserUpdatedConsumer(
    ILogger<CompanyUserUpdatedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, message, validate, mapper)
{
}