using AutoMapper;
using Journey.Core.Services.Quote;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Journey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuoteController : BaseJourneyController
    {
        public QuoteController(IMediator mediator, IMapper mapper):base(mediator, mapper)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetQuote()
        {
            var user = GetLoggedInUser();
            return new JsonResult(await _mediator.Send(new GetDailyQuoteRequest(user.UserId)));
        }

    }
}
