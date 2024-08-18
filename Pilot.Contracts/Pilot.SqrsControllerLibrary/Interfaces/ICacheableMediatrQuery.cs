using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface ICacheableOneMediatrQuery
{
    string CacheKey { get; }
}

public interface ICacheableListMediatrQuery
{
    public BaseFilter Filter { get; set; }

    string CacheKey { get; }
}