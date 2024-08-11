using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Data;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Service;

public class ValidatorService(
    IMessageService message,
    IUserService user,
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService<CompanyUser>(message, user, logger, context)
{

}