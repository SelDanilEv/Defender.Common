namespace Defender.Common.Cache;

public static class CacheConventionBuilder
{
    public static string BuildDistributedCacheKey(
        CacheForService service, CacheModel model, string uniqueKey)
        => $"{service}_{model}_{uniqueKey}";
}