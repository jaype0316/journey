using AutoMapper;
using Journey.Core.Providers;
using Journey.Core.Services.Blobs;
using Journey.Core.Services.Books;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Journey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : BaseJourneyController
    {
        private readonly IBlobStorageService _blobStorage;
        private readonly IBlobKeyProvider _blobObjectKeyProvider;

        public BookController(IMediator mediator, IBlobStorageService blobStorage, IBlobKeyProvider keyProvider, IMapper mapper) :base(mediator, mapper)
        {
            this._blobStorage = blobStorage;
            this._blobObjectKeyProvider = keyProvider;
        }

        [HttpGet]
        public async Task<Models.DTO.Book> Get()
        {
            //var email = User.Identity.Name;
            var user = GetLoggedInUser();
            return await _mediator.Send(new GetBookRequest(user.UserId));
        }

        [HttpGet("getLogo")]
        public async Task<IActionResult> GetLogo()
        {
            var user = GetLoggedInUser();
            var book = await _mediator.Send(new GetBookRequest(user.UserId));
            if (string.IsNullOrEmpty(book?.LogoKey)) return new NoContentResult();

            var response = await _blobStorage.Get(book.LogoKey);

            return new FileStreamResult(response.Item1, response.Item2);
        }

        [HttpPost, Route("save")]
        public async Task<IActionResult> Save(SaveBookCommand command)
        {
            command.UserId = GetLoggedInUser().UserId;
            return new JsonResult(await _mediator.Send(command));
        }

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo(IFormFile file)
        {
            if (file.Length == 0) return new NoContentResult();

            var user = GetLoggedInUser();
            var key = _blobObjectKeyProvider.Provide(file.FileName);
            var success = await _blobStorage.Add(key, file);
            if(!success) return new NoContentResult();

            var book = await _mediator.Send(new GetBookRequest(user.UserId));         
            book.LogoKey = key;

            await _mediator.Send(_mapper.Map<SaveBookCommand>(book));
                  
            return new JsonResult(key);
            
        }
    }
}
