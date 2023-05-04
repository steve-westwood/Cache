public class CacheEntry
{
    public string? Key;
    public string? Value;
    public CacheEntry? Before { get; set; }
    public CacheEntry? After { get; set; }
    public CacheEntry() {}
    public CacheEntry(string cacheKey, string cacheValue)
    {
        Key = cacheKey;
        Value = cacheValue;
    }
}
