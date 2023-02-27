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
        Task<T> GetOrAdd<T>(string key, Func<Task<T>> action, int expirationMinutes);
        Task Invalidate(string key);
        Task<T> Get<T>(string key);
        
    }
    public class CacheProvider : ICacheProvider
    {
        readonly IMemoryCache _cache;
        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> action, int expirationMinutes)
        {
            if(!_cache.TryGetValue(key, out var result))
            {
                var item = await action();
                if (!EqualityComparer<T>.Default.Equals(item, default(T)))
                {
                    _cache.Set<T>(key, item, DateTimeOffset.UtcNow.AddMinutes(expirationMinutes));
                }
            }
            return _cache.Get<T>(key);
        }

        public async Task<T> Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public Task Invalidate(string key)
        {
            if (_cache.TryGetValue(key, out var value))
                _cache.Remove(key);

            return Task.CompletedTask;
        }
    }
}
