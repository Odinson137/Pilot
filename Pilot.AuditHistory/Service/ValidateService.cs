using Pilot.AuditHistory.Data;
using Pilot.AuditHistory.Interface;
using Pilot.Contracts.Base;

namespace Pilot.AuditHistory.Service;

public class ValidatorService(
    ILogger<ValidatorService> logger,
    ClickHouseContext context)
    : BaseValidateService(logger, context), IValidatorService
{
}