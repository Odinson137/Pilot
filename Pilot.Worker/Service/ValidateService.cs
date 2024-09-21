using Pilot.Contracts.Base;
using Pilot.Worker.Data;

namespace Pilot.Worker.Service;

public class ValidatorService(
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(logger, context)
{
}