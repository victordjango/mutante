using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace myMicroservice.Caching
{

    public class SimpleMemoryCache<TItem>
    {
        private MemoryCache _cache;
        public SimpleMemoryCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions() { ExpirationScanFrequency = new TimeSpan(1, 0, 0) });
        }
        public TItem GetOrCreate(object key, Func<TItem> createItem)
        {
            TItem cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))// Look for cache key.
            {   
                cacheEntry = createItem();

                // Save data in cache.
                _cache.Set(key, cacheEntry);
            }
            return cacheEntry;
        }

        public bool IsInCache(object key)
        {
            TItem cacheEntry;

            if (_cache.TryGetValue(key,out cacheEntry))
            {
                return true;
            }
            return false;
        }
    }

}
