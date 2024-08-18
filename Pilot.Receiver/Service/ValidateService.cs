using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Data;

namespace Pilot.Receiver.Service;

public class ValidatorService(
    IMessageService message,
    IModelService user,
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(message, user, logger, context)
{
}