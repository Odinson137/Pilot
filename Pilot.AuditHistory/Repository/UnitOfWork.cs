using Pilot.AuditHistory.Data;
using Pilot.SqrsControllerLibrary.Repositories;

namespace Pilot.AuditHistory.Repository;

public class UnitOfWork(ILogger<UnitOfWork> logger, ClickHouseContext context) : BaseUnitOfWork(logger, context);
