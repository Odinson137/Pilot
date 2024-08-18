namespace Pilot.InvalidationCacheRedisLibrary.CacheKeyTemplates;

public static class BaseCacheKeyTemplate
{
    public static string OneCacheKey(string modelName, int id) => $"{modelName}-{id}";
    
    public static string GroupListCacheKey(string modelName) => modelName;
    
    public static string ListCacheKey(string modelName, int skip, int take) =>  $"all-{modelName}-{skip}-{take}";
}