using Pilot.AuditHistory.Interface;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.AuditHistory.Handlers;

public class AuditHistoryQueryHandler(IAuditHistory repository, ILogger<AuditHistoryQueryHandler> logger)
    : ModelQueryHandler<Models.AuditHistory, AuditHistoryDto>(repository, logger);