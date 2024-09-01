using Pilot.Contracts.Base;
using Pilot.Receiver.Data;

namespace Pilot.Receiver.Service;

public class ValidatorService(
    IModelService user,
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(logger, context)
{
}