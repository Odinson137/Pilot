using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Validation;
using Pilot.Messenger.Data;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Services;

public class ValidatorService(
    IMessageService message,
    IUserService user,
    ILogger<ValidateError> logger,
    DataContext context)
    : BaseValidateService<MessageUser>(message, user, logger, context)
{

}