using Journey.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core
{
    public static class UserContextCache
    {
        private static readonly ConcurrentDictionary<string, UserContext> _cache = new ConcurrentDictionary<string, UserContext>();

        public static UserContext Get(string key) 
        { 
            _cache.TryGetValue(key, out var userContext);
            return userContext;
        }

        public static void Clear(string key)
        {
            _cache.TryRemove(key, out var userContext);
        }

        public static void Clear()
        {
            _cache.Clear();
        }

        public static bool Contains(string key)
        {
            return _cache.ContainsKey(key);
        }

        public static void Set(string key,  UserContext value)
        {
            if (!_cache.ContainsKey(key))
            {
                _cache.TryAdd(key, value);
            } else
            {
                _cache[key] = value;    
            }
        }
    }
}
