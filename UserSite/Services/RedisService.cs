using Microsoft.Extensions.Caching.Distributed;

namespace UserSite.Services;

public class RedisService
{
    private readonly IDistributedCache _distributedCache;
        
    public RedisService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task SetValue(string key, string value,int numDay)
    {
        var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(numDay));
        await _distributedCache.SetStringAsync(key, value, options);
    }

    public async Task<string?> GetValue(string? key)
    {
        return await _distributedCache.GetStringAsync(key);
    }
    public async Task Remove(string? key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}