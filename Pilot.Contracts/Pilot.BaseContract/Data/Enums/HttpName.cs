namespace Pilot.Contracts.Data.Enums;

public enum ServiceName
{
    None = -1,
    
    ApiServer = 0,
    
    IdentityServer = 1,

    WorkerServer = 2,

    MessengerServer = 3,
    
    StorageServer = 4,
    
    CapabilityServer = 5,

    BackgroundJobService = 6,
    
    AuditHistoryService = 7,
}