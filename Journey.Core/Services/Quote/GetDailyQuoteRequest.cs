using AutoMapper;
using Journey.Core.Repository;
using Journey.Core.Services.Quote.ApiNinja;
using Journey.Core.Services.Quote.ZenQuote;
using Journey.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote
{
    public class GetDailyQuoteRequest : IRequest<Journey.Models.DTO.Quote>
    {
        public string UserId { get; private set; }
        public GetDailyQuoteRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class GetDailyQuoteRequestHandler : IRequestHandler<GetDailyQuoteRequest, Journey.Models.DTO.Quote>
    {
        readonly IRepository _repository;
        readonly IZenQuoteClientHandler _zenQuoteClientHandler;
        readonly IApiNinjaClientHandler _ninjaQuoteClientHandler;
        readonly IQuoteProvider _quoteProvider;
        readonly IMapper _mapper;
        const string QUOTES_CACHE_KEY = "journey_quotes";

        public GetDailyQuoteRequestHandler(IZenQuoteClientHandler zenQuoteClientHandler, IRepository repository, IQuoteProvider quoteProvider,
            IApiNinjaClientHandler ninjaQuoteClientHandler, IMapper mapper)
        {
            _zenQuoteClientHandler = zenQuoteClientHandler;
            _repository = repository;
            _quoteProvider = quoteProvider;
            _mapper = mapper;
            _ninjaQuoteClientHandler = ninjaQuoteClientHandler;
        }

        public async Task<Journey.Models.DTO.Quote> Handle(GetDailyQuoteRequest request, CancellationToken cancellationToken)
        {
            var zenQuotes = _mapper.Map<IEnumerable<Journey.Models.DTO.Quote>>(await _zenQuoteClientHandler.GetQuotes());
            var ninjaQuotes = _mapper.Map<IEnumerable<Journey.Models.DTO.Quote>>(await _ninjaQuoteClientHandler.GetQuotes());
            var combinedQuotes = zenQuotes.Union(ninjaQuotes, new QuoteComparer());
            if (combinedQuotes == null)
                throw new InvalidOperationException("There are no quotes");

            var userQuotes = await _repository.GetAsync<UserQuote>(request.UserId, request.UserId) ?? new UserQuote();
            var randomQuote = _quoteProvider.Provide(combinedQuotes.ToHashSet(), userQuotes.Quotes);

            if (!userQuotes.Quotes.Any())
            {
                userQuotes = new UserQuote()
                {
                    UserId = request.UserId
                };
                userQuotes.Quotes.Add(new UserQuote.Quote()
                {
                    Author = randomQuote.Author,
                    Text = randomQuote.Text,
                    LastShownAt = DateTime.UtcNow
                });
                await _repository.CreateAsync<UserQuote>(userQuotes);
            } 
            else
            {
                userQuotes.Quotes.Add(new UserQuote.Quote()
                {
                    Author = randomQuote.Author,
                    Text = randomQuote.Text,
                    LastShownAt = DateTime.UtcNow
                });
                await _repository.UpdateAsync<UserQuote>(userQuotes);
            }

            return randomQuote;
        }
    }
}
