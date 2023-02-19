using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote.ApiNinja
{

    public interface INinjaQuoteClient
    {
        Task<IEnumerable<NinjaQuoteDTO>> GetQuotes();
    }
    internal class NinjaQuoteClient : INinjaQuoteClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NinjaQuoteClient> _logger;

        public NinjaQuoteClient(IHttpClientFactory httpClientFactory, ILogger<NinjaQuoteClient> logger)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;
        }

        public async Task<IEnumerable<NinjaQuoteDTO>> GetQuotes()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.api-ninjas.com/v1/quotes?limit=10");
            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"ZenQuoteClient.GetQuotes: {response.StatusCode}. {response.ReasonPhrase}");
                return Enumerable.Empty<NinjaQuoteDTO>();
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<NinjaQuoteDTO>>();
        }
    }
}
