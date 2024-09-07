using Pilot.Contracts.Base;
using Pilot.Storage.Data;

namespace Pilot.Storage.Service;

public class StorageValidateService(ILogger<BaseValidateService> logger, DataContext context)
    : BaseValidateService(logger, context);