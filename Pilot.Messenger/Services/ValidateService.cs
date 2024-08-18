using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Data;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Services;

public class ValidatorService(
    IMessageService message,
    IModelService user,
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(message, user, logger, context)
{
}