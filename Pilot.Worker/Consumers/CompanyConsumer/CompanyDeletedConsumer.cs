using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyDeletedConsumer(
    ILogger<CompanyDeletedConsumer> logger,
    ICompany company,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<Models.Company, CompanyDto>(logger, company, message, validate)
{
}