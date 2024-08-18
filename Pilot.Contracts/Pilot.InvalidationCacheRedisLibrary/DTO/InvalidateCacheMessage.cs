using Pilot.Contracts.Data.Enums;

namespace Pilot.InvalidationCacheRedisLibrary.DTO;

public class InvalidateCacheMessage
{
    public string Type { get; set; } = null!;  
    
    public ActionState Operation { get; set; }  
    
    public int Id { get; set; }
}