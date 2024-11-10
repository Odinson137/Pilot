using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Service;

public class ValidatorService(
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(logger, context), IValidatorService
{
}