using Pilot.AuditHistory.Interface;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.AuditHistory.Handlers;

public class AuditHistoryQueryHandler : ModelQueryHandler<Models.AuditHistory, AuditHistoryDto>
{
    public AuditHistoryQueryHandler(IAuditHistory repository, ILogger<AuditHistoryQueryHandler> logger) : base(repository, logger)
    {
    }
}