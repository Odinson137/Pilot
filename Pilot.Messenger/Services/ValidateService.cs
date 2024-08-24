using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Data;

namespace Pilot.Messenger.Services;

public class ValidatorService(
    IMessageService message,
    IModelService user,
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(user, logger, context)
{
}