namespace Pilot.AuditHistory.Interface;

public interface IClickHouseService
{
    Task ExecuteAsync(string query, CancellationToken cancellationToken = default);
    Task InsertAuditLogAsync(Models.AuditHistory auditLog, CancellationToken cancellationToken = default);
}