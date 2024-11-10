using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyRoleConsumer;

public class CompanyRoleDeletedConsumer(
    ILogger<CompanyRoleDeletedConsumer> logger,
    ICompanyRole repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<CompanyRole, CompanyRoleDto>(logger, repository, message, validate)
{
}