using Microsoft.EntityFrameworkCore;
using Pilot.AuditHistory.Data;
using Pilot.AuditHistory.Interface;

namespace Pilot.AuditHistory.Service;

public class ClickHouseService(ClickHouseContext context) : IClickHouseService
{
    public async Task ExecuteAsync(string query, CancellationToken cancellationToken = default)
    {
        await context.Database.ExecuteSqlRawAsync(query, cancellationToken);
    }

    public async Task InsertAuditLogAsync(Models.AuditHistory auditLog, CancellationToken cancellationToken = default)
    {
        context.AuditHistories.Add(auditLog);
        await context.SaveChangesAsync(cancellationToken);
    }
}