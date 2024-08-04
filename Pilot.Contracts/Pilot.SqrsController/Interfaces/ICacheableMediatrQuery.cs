using Pilot.Contracts.Base;

namespace Pilot.SqrsController.Interfaces;

public interface ICacheableMediatrQuery 
{
    public BaseFilter? Filter { get; set; }
    string CacheKey { get; }
    // TimeSpan? SlidingExpiration { get; }
}