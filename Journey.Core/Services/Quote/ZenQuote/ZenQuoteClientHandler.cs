using Journey.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote.ZenQuote
{
    public interface IZenQuoteClientHandler
    {
        public Task<IEnumerable<ZenQuoteDto>> GetQuotes();
    }
    public class ZenQuoteClientHandler : IZenQuoteClientHandler
    {
        readonly IZenQuoteClient _client;
        readonly ICacheProvider _cache;
        const string QUOTES_CACHE_KEY = "journey_zenquotes";
        public ZenQuoteClientHandler(IZenQuoteClient zenQuoteClient, ICacheProvider cache)
        {
            _client = zenQuoteClient;
            _cache = cache;
        }

        public async Task<IEnumerable<ZenQuoteDto>> GetQuotes()
        {
            return await _cache.GetOrAdd(QUOTES_CACHE_KEY, async () =>
            {
                return await _client.GetQuotes();
            });
        }
    }
}
