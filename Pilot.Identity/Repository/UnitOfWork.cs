using Pilot.Identity.Data;
using Pilot.SqrsControllerLibrary.Repositories;

namespace Pilot.Identity.Repository;

public class UnitOfWork(ILogger<UnitOfWork> logger, DataContext context) : BaseUnitOfWork(logger, context);
