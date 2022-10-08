using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote
{
    public interface IQuoteProvider
    {
        Journey.Models.DTO.Quote Provide(ISet<Journey.Models.DTO.Quote> candidateQuotes, IEnumerable<Journey.Models.DTO.UserQuote.Quote> userQuotes);
    }
}
