using Journey.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote.ApiNinja
{
    public interface IApiNinjaClientHandler
    {
        Task<IEnumerable<NinjaQuoteDTO>> GetQuotes();
    }
    public class ApiNinjaClientHandler : IApiNinjaClientHandler
    {
        readonly INinjaQuoteClient _client;
        readonly ICacheProvider _cache;
        const string NINJA_QUOTES_CACHE_KEY = "journey_ninja_quotes";
        public ApiNinjaClientHandler(INinjaQuoteClient client, ICacheProvider cache)
        {
            _client = client;
            _cache = cache;
        }
        public async Task<IEnumerable<NinjaQuoteDTO>> GetQuotes()
        {
            return  await _cache.GetOrAdd(NINJA_QUOTES_CACHE_KEY, async () =>
            {
                return await _client.GetQuotes();
            }, 1440);
        }
    }
}
