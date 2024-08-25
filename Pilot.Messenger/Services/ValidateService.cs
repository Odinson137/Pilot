using Pilot.Contracts.Base;
using Pilot.Messenger.Data;

namespace Pilot.Messenger.Services;

public class ValidatorService(
    IModelService user,
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(user, logger, context)
{
}