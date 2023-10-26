using Microsoft.Extensions.Caching.Distributed;

namespace UserSite.Controllers;


using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
public class RedisController : Controller
{
    private readonly IDistributedCache _distributedCache;
        
    public RedisController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
        
    [HttpGet]
    public string Get()
    {
        var cacheKey = "TheTime";
        var currentTime = DateTime.Now.ToString();
        var cachedTime = _distributedCache.GetString(cacheKey);
        if(string.IsNullOrEmpty(cachedTime))
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(5));
            // Nạp lại giá trị mới cho cache
            _distributedCache.SetString(cacheKey, currentTime, options);
            cachedTime = _distributedCache.GetString(cacheKey);
        }
        var result = $"Current Time : {currentTime} \nCached  Time : {cachedTime}";
        return result;
    }
}