using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyUserConsumer;

public class CompanyUserDeletedConsumer(
    ILogger<CompanyUserDeletedConsumer> logger,
    ICompanyUser companyUser,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<CompanyUser, CompanyUserDto>(logger, companyUser, message, validate);