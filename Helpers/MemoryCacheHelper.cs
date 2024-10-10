using Microsoft.Extensions.Caching.Memory;

namespace LocalizationWithJsonResource.Helpers
{
    public class MemoryCacheHelper
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Cache'de var mı kontrol eder, yoksa veriyi ekleyip geri döner.
        /// </summary>
        public T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan expirationTime)
        {
            // Eğer cache'de var ise direkt döndür
            if (_memoryCache.TryGetValue(key, out T cacheItem))
            {
                return cacheItem;
            }

            // Cache'de yok ise, oluştur ve cache'e ekle
            cacheItem = createItem();
            _memoryCache.Set(key, cacheItem, expirationTime);
            return cacheItem;
        }

        /// <summary>
        /// Cache'den veriyi getirir.
        /// </summary>
        public T Get<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T cacheItem) ? cacheItem : default;
        }

        /// <summary>
        /// Cache'e veri ekler.
        /// </summary>
        public void Set<T>(string key, T item, TimeSpan expirationTime)
        {
            _memoryCache.Set(key, item, expirationTime);
        }

        /// <summary>
        /// Cache'den veriyi siler.
        /// </summary>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
