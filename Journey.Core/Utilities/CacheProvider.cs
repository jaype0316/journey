using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Utilities
{
    public interface ICacheProvider
    {
        public Task<T> GetOrAdd<T>(string key, Func<Task<T>> action);
        public Task Invalidate(string key);
    }
    public class CacheProvider : ICacheProvider
    {
        readonly IMemoryCache _cache;
        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetOrAdd<T>(string key, Func<Task<T>> action)
        {
            return _cache.GetOrCreateAsync<T>(key, option =>
            {
                option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return action();
            });

        }

        public Task Invalidate(string key)
        {
            if (_cache.TryGetValue(key, out var value))
                _cache.Remove(value);

            return Task.CompletedTask;
        }
    }
}
