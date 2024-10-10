using Microsoft.Extensions.Caching.Memory;

namespace LocalizationWithJsonResource.Helpers
{
    public class MemoryCacheHelper
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? GetFromCache<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void SetCache<T>(string key, T value, TimeSpan expirationTime)
        {
            _cache.Set(key, value, expirationTime);
        }
    }
}
