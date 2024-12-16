using Pilot.BackgroundJob.Data;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Base;

namespace Pilot.BackgroundJob.Service;

public class ValidatorService(
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(logger, context), IValidatorService
{
}