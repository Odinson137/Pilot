using Pilot.Contracts.Base;
using Pilot.Messenger.Data;

namespace Pilot.Messenger.Services;

public class MessengerValidateService(ILogger<BaseValidateService> logger, DataContext context)
    : BaseValidateService(logger, context);