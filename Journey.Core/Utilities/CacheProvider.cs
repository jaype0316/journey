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

        public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> action)
        {
            if(!_cache.TryGetValue(key, out var result))
            {
                var item = await action();
                if (!EqualityComparer<T>.Default.Equals(item, default(T)))
                {
                    _cache.Set<T>(key, item);
                }
            }
            return _cache.Get<T>(key);
        }

        public Task Invalidate(string key)
        {
            if (_cache.TryGetValue(key, out var value))
                _cache.Remove(value);

            return Task.CompletedTask;
        }
    }
}
