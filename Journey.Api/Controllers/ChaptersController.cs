using AutoMapper;
using Journey.Core.Providers;
using Journey.Core.Repository;
using Journey.Core.Services.Blobs;
using Journey.Core.Services.Chapters;
using Journey.Core.Services.Chapters.Notifications;
using Journey.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Journey.Api.Controllers
{
    [Route("api/chapter")]
    [ApiController]
    [Authorize]
    public class ChaptersController : BaseJourneyController
    {
        private readonly IBlobKeyProvider _blobKeyProvider;
        private readonly IBlobStorageService _blobStorageService;

        public ChaptersController(IMediator mediator, IMapper mapper, IBlobKeyProvider blobKeyProvider, IBlobStorageService blobStorageService) :base(mediator, mapper)
        {
            this._blobKeyProvider = blobKeyProvider;
            this._blobStorageService = blobStorageService;
        }

        [HttpGet, Route("get/{pk}")]
        public async Task<Journey.Models.DTO.Chapter> Get(string pk)
        {
            var user = GetLoggedInUser();
            return await _mediator.Send(new GetById(pk, user.UserId));
        }

        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save(SaveChapterCommand command)
        {
            var user = GetLoggedInUser();

            command.UserId = user.UserId;
            
            var chapterId = await _mediator.Send(command);
            
            await _mediator.Publish(new ChapterSavedNotification(user.UserId));
            
            return new JsonResult(chapterId);
        }

        [HttpGet, Route("list")]
        public async Task<IEnumerable<Journey.Core.ViewModels.Chapter>> GetList()
        {
            var user = GetLoggedInUser();
            return await _mediator.Send(new GetListRequest(user.UserId));
        }

        [HttpGet, Route("listheaders")]
        public async Task<IEnumerable<ChapterHeader>> GetListHeaders([FromQuery] string? orderBy)
        {
            var user = GetLoggedInUser();
            return await _mediator.Send(new GetHeadersListRequest(user.UserId, orderBy));
        }

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo(IFormFile file, string chapterKey)
        {
            if (file.Length == 0) return new NoContentResult();

            var user = GetLoggedInUser();
            var blobObjectKey = _blobKeyProvider.Provide(file.FileName);
            var success = await _blobStorageService.Add(blobObjectKey, file);

            if (!success) return new NoContentResult();

            var chapter = await _mediator.Send(new GetById(chapterKey, user.UserId));
            chapter.LogoKey = blobObjectKey;

            await _mediator.Send(_mapper.Map<SaveChapterCommand>(chapter));


            return new JsonResult(success);

        }
    }
}
