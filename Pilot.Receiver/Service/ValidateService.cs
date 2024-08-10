using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Contracts.Validation;
using Pilot.Receiver.Data;

namespace Pilot.Receiver.Service;

public class ValidatorService(
    IMessageService message,
    IUserService user,
    ILogger<ValidateError> logger,
    DataContext context)
    : BaseValidateService<CompanyUser>(message, user, logger, context)
{

}