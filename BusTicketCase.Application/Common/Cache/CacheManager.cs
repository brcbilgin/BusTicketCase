using Microsoft.Extensions.Caching.Memory;

namespace BusTicketCase.Application.Common.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public async Task Set<T>(string key, T value)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1),
                Priority = CacheItemPriority.Normal
            });
        }
    }
}
