using Journey.Core.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote.ZenQuote
{
    public interface IZenQuoteClient
    {
        Task<IEnumerable<ZenQuoteDto>> GetQuotes();
    }
    public class ZenQuoteClient : IZenQuoteClient
    {
        readonly IHttpClientFactory _clientFactory;
        readonly ILogger<ZenQuoteClient> _logger;
        public ZenQuoteClient(IHttpClientFactory clientFactory, ILogger<ZenQuoteClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<ZenQuoteDto>> GetQuotes()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://zenquotes.io/api/quotes/");
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"ZenQuoteClient.GetQuotes: {response.StatusCode}. {response.ReasonPhrase}");
                return Enumerable.Empty<ZenQuoteDto>();
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<ZenQuoteDto>>();
        }
    }
}
