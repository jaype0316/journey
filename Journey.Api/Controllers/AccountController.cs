using AutoMapper;
using Journey.Core.Providers;
using Journey.Core.Repository;
using Journey.Core.Services.Blobs;
using Journey.Core.Services.Books;
using Journey.Core.Services.Chapters;
using Journey.Core.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace Journey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseJourneyController
    {
        private readonly IBlobStorageService _blobStorage;
        private readonly IBlobKeyProvider _blobObjectKeyProvider;

        public AccountController(IMediator mediator, IBlobStorageService blobStorage, IBlobKeyProvider keyProvider, IMapper mapper) :base(mediator, mapper)
        {
            this._blobStorage = blobStorage;
            this._blobObjectKeyProvider = keyProvider;
        }


        [HttpGet, Route("AssertUser")]
        public async Task<Journey.Models.User> AssertUserExists()
        {
            var user = GetLoggedInUser();
            var assertedUser = await _mediator.Send(new GetOrAddRequest(user.UserId, user.Email));
            return assertedUser;
            //return await _mediator.Send(new GetBookRequest() { UserId = _userId});
        }

        [HttpPost]
        public async Task Register()
        {

        }

        [HttpGet, Route("Bootstrap")]
        public async Task<IActionResult> Bootstrap()
        {
            var user = GetLoggedInUser();
            await _mediator.Send(new Core.Services.User.GetOrAddRequest(user.UserId, user.Email));

            var book = await _mediator.Send(new Core.Services.Books.GetBookRequest(user.UserId));
            if(book == null)
                book = await _mediator.Send(new SaveBookCommand(user.UserId) { Title = "My Journey", About = "What's on your mind...?", UserId = user.UserId });
            
            return new JsonResult(book);

        }

    }
}
