using Journey.Core.Repository;
using Journey.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Repository.Memory
{
    public class InMemoryRepository : IReadRepository
    {
        readonly IMemoryCache _cache;
        public InMemoryRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetAsync<T>(string id)
        {
            _cache.TryGetValue<T>(id, out var result);
            return Task.FromResult(result);
        }
    }
}
