using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public class CacheWrapper : ICacheWrapper
    {
        private readonly IMemoryCache _cache;
        public CacheWrapper(IMemoryCache cache)
        {
            _cache = cache;
        }
        public bool IsAvailableInCache<T>(string key, out T value)
        {
            bool exists = _cache.TryGetValue<T>(key, out T cacheValue);
            value = cacheValue;
            return exists;
        }

        public T Set<T>(string key, T value, MemoryCacheEntryOptions options)
        {
            return _cache.Set(key, value, options);
        }
    }
}
