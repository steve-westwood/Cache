using System.Text.Json;

namespace Caching.Core;
public sealed class Cache
{
    private const int CACHE_CAPACITY = 100;
    private Dictionary<string, CacheEntry> _lookup;
    private CurrentCache _currentCache;
    private static readonly Lazy<Cache> lazy
        = new Lazy<Cache>(() => new Cache());

    public static Cache Instance
        => lazy.Value;

    private Cache() 
    {
        _lookup = new Dictionary<string, CacheEntry>();
        _currentCache = new CurrentCache();
    }
    public void Set<T>(string cacheKey, T cacheItem)
    {
        if (cacheItem == null) return;
        if (_lookup.ContainsKey(cacheKey)) 
        {
            _currentCache.RemoveNode(_lookup[cacheKey]);
            _lookup.Remove(cacheKey);
        }
        var cacheEntry = new CacheEntry(cacheKey, JsonSerializer.Serialize(cacheItem));
        _currentCache.AddToTop(cacheEntry);
        _lookup.Add(cacheKey, cacheEntry);
        if (_lookup.Count > CACHE_CAPACITY)
        {
            var evicted = _currentCache.RemoveCacheEntry();
            if (evicted?.Key != null) _lookup.Remove(evicted.Key);
        } 
    }
    public T? Get<T>(string cacheKey)
    {
        if (!_lookup.ContainsKey(cacheKey)) return default(T);
        var cacheEntry = _lookup[cacheKey];
        _currentCache.AddToTop(cacheEntry);
        return (cacheEntry.Value != null) 
            ? JsonSerializer.Deserialize<T>(cacheEntry.Value)
            : default(T);
    }
    public void Flush()
    {
        _lookup = new Dictionary<string, CacheEntry>();
        _currentCache = new CurrentCache();
    }
}