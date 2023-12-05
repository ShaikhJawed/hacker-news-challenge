using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public interface ICacheWrapper
    {
        bool IsAvailableInCache<T>(string key, out T value);
        T Set<T>(string key, T value, MemoryCacheEntryOptions options);

    }
}
