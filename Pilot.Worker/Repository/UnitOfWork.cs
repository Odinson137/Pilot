using Pilot.SqrsControllerLibrary.Repositories;
using Pilot.Worker.Data;

namespace Pilot.Worker.Repository;

public class UnitOfWork(ILogger<UnitOfWork> logger, DataContext context) : BaseUnitOfWork(logger, context);
