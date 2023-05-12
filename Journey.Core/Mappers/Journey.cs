using AutoMapper;
using Journey.Core.Services.Books;
using Journey.Core.Services.Chapters;
using Journey.Core.Services.Quote.ApiNinja;
using Journey.Core.Services.Quote.ZenQuote;
using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Journey.Core.Services.Quote.GetDailyQuoteRequestHandler;
using static Journey.Core.Services.Quote.RandomQuoteProvider;

namespace Journey.Core.Mappers
{
    public class JourneyMappers : Profile
    {
        public JourneyMappers()
        {
            CreateMap<SaveChapterCommand, Chapter>().ForMember(c => c.LogoKey, c => c.MapFrom(x => x.Thumbnail)).ReverseMap();
            CreateMap<SaveBookCommand, Book>().ForMember(c => c.LogoKey, c => c.MapFrom(x => x.Thumbnail)).ReverseMap();
            CreateMap<Quote, QuoteComparee>().ReverseMap();
            CreateMap<UserQuote, QuoteComparee>().ReverseMap();
            CreateMap<UserQuote, Quote>().ReverseMap();
            CreateMap<ZenQuoteDto, Quote>().ForMember(c => c.Text, x => x.MapFrom(x => x.Q))
                                            .ForMember(c => c.Author, x => x.MapFrom(x => x.A));
            CreateMap<UserQuote.Quote, QuoteComparee>().ReverseMap();
            CreateMap<Journey.Models.DTO.Quote, Journey.Models.DTO.UserQuote.Quote>().ReverseMap();

            //API Ninja
            CreateMap<NinjaQuoteDTO, Quote>().ForMember(c => c.Text, x => x.MapFrom(x => x.Quote)).ReverseMap();
        }
    }
}
