using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using TechShop.Application.Services.AppServices.CacheService;

public class CacheService(IMemoryCache cache) : ICacheService
{
    public T? Get<T>(string key)
    {
        cache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan expirationTime, TimeSpan slidingExpiration)
    {
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime,
            SlidingExpiration = slidingExpiration,
        };

        cache.Set(key, value, cacheOptions);
    }

    public void Remove(string key)
    {
        cache.Remove(key);
    }
}
