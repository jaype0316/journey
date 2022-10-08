using AutoMapper;
using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote
{
    public class RandomQuoteProvider : IQuoteProvider
    {
        readonly IMapper _mapper;
        public RandomQuoteProvider(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Journey.Models.DTO.Quote Provide(ISet<Journey.Models.DTO.Quote> candidateQuotes, IEnumerable<UserQuote.Quote> userQuotes)
        {
            var randomizer = new Random();
            var existingUserQuotes = _mapper.Map<IEnumerable<QuoteComparee>>(userQuotes).ToHashSet();
            var preliminaryCandidates = _mapper.Map<IEnumerable<QuoteComparee>>(candidateQuotes).ToHashSet();

            preliminaryCandidates.ExceptWith(existingUserQuotes);

            if (preliminaryCandidates.Any())
            {
                var randomQuote = preliminaryCandidates.ElementAt(randomizer.Next(0, preliminaryCandidates.Count - 1));
                return _mapper.Map<Journey.Models.DTO.Quote>(randomQuote);
            }

            //if we got here, we need to look at the user quotes' dates, return one that hasn't been shown in 30 days or more
            var oldestUserQuote = userQuotes.OrderBy(c => c.LastShownAt).FirstOrDefault();
            return _mapper.Map<Journey.Models.DTO.Quote>(oldestUserQuote);
        }

        public class QuoteComparee
        {
            public string Author { get; set; }
            public string Text { get; set; }
        }
    }
}
